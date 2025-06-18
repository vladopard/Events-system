using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Events_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly IMapper _mapper;

        public TicketsController(ITicketService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAll()
        {
            var tickets = await _service.GetAllAsync();
            return Ok(tickets);
        }

        // GET: api/tickets/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TicketDTO>> GetById(int id)
        {
            var ticket = await _service.GetByIdAsync(id);
            return Ok(ticket);
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> Create([FromBody] TicketCreateDTO dto)
        {
            var ticket = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }

        // PUT: api/tickets/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TicketUpdateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // PATCH: api/tickets/{id}
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TicketPatchDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var ticketDto = await _service.GetByIdAsync(id);
            var ticketForPatch = _mapper.Map<TicketPatchDTO>(ticketDto);

            patchDoc.ApplyTo(ticketForPatch, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (!TryValidateModel(ticketForPatch))
                return UnprocessableEntity(ModelState);

            await _service.PatchAsync(id, ticketForPatch);
            return NoContent();
        }

        // DELETE: api/tickets/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
