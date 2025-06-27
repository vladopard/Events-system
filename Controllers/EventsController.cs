using System.Text.Json;
using AutoMapper;
using Events_system.BusinessServices;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Events_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public EventsController(IEventService service, IMapper mapper, IDistributedCache cache)
        {
            _service = service;
            _mapper = mapper;
            _cache = cache;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<EventDTO>>> GetAll()
        //{
        //    var events = await _service.GetAllAsync();
        //    return Ok(events);
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAll([FromQuery] EventQueryParameters p)
        {
            var cacheKey = $"events_page_{p.PageNumber}_{p.PageSize}_{p.Search}";
            var camelOpts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            // pokušaj iz keša
            var cachedJson = await _cache.GetStringAsync(cacheKey);
            if (cachedJson != null)
            {
                Console.WriteLine("➡️ Vraćam iz Redis keša");
                var cachedPage = JsonSerializer
                    .Deserialize<CachedPageDto<EventDTO>>(cachedJson, camelOpts)
                    ?? throw new InvalidOperationException("Nešto nije u redu sa keširanim podacima");

                Response.Headers.Append("X-Pagination",
                    JsonSerializer.Serialize(cachedPage.MetaData, camelOpts));

                // vraćamo samo listu
                return Ok(cachedPage.Items);
            }

            // MISS
            Response.Headers.Append("X-Cache", "MISS");

            // povuci iz servisa
            var result = await _service.GetAllAsync(p);

            // upakuj u svoj DTO, obavezno ToList()
            var toCache = new CachedPageDto<EventDTO>
            {
                Items = result.ToList(),
                MetaData = result.MetaData
            };

            var cacheOpts = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            var jsonToCache = JsonSerializer.Serialize(toCache, camelOpts);
            await _cache.SetStringAsync(cacheKey, jsonToCache, cacheOpts);

            // header za paginaciju
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(result.MetaData, camelOpts));

            // vraćamo listu, ne PagedList
            return Ok(result.ToList());
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDTO>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<EventDTO>> Create([FromBody] EventCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, EventUpdateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
            //FIX separate message for not found
            //UPSERTING NOT ALLOWED YET
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(
            int id, JsonPatchDocument<EventPatchDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            // Fetch existing event as DTO
            var eventDto = await _service.GetByIdAsync(id);
            var eventForPatch = _mapper.Map<EventPatchDTO>(eventDto);

            patchDoc.ApplyTo(eventForPatch, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (!TryValidateModel(eventForPatch))
                return UnprocessableEntity(ModelState);

            await _service.PatchAsync(id, eventForPatch);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
