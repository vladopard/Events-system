using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Events_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypesController : ControllerBase
    {
        private readonly ITicketTypeService _service;
        private readonly IMapper _mapper;

        public TicketTypesController(ITicketTypeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/tickettypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketTypeDTO>>> GetAll()
        {
            var types = await _service.GetAllAsync();
            return Ok(types);
        }

        // GET: api/tickettypes/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TicketTypeDTO>> GetById(int id)
        {
            var type = await _service.GetByIdAsync(id);
            return Ok(type);
        }

        // GET: api/tickettypes/by-event/{eventId}
        [HttpGet("by-event/{eventId:int}")]
        public async Task<ActionResult<IEnumerable<TicketTypeDTO>>> GetByEventId(int eventId)
        {
            var types = await _service.GetByEventIdAsync(eventId);
            return Ok(types);
        }

        // POST: api/tickettypes
        [HttpPost]
        public async Task<ActionResult<TicketTypeDTO>> Create([FromBody] TicketTypeCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/tickettypes/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TicketTypeUpdateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // PATCH: api/tickettypes/{id}
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TicketTypePatchDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var current = await _service.GetByIdAsync(id);
            var patchDto = _mapper.Map<TicketTypePatchDTO>(current);

            patchDoc.ApplyTo(patchDto, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (!TryValidateModel(patchDto))
                return UnprocessableEntity(ModelState);

            await _service.PatchAsync(id, patchDto);
            return NoContent();
        }

        // DELETE: api/tickettypes/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
