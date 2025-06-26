using System.Text.Json;
using AutoMapper;
using Events_system.BusinessServices;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Events_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IMapper _mapper;

        public EventsController(IEventService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<EventDTO>>> GetAll()
        //{
        //    var events = await _service.GetAllAsync();
        //    return Ok(events);
        //}
        [HttpGet]
        public async Task<ActionResult<PagedList<EventDTO>>> GetAll(
            [FromQuery] EventQueryParameters p)
        {
            var page = await _service.GetAllAsync(p);

            var camel = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(page.MetaData, camel));


            return Ok(page);
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
