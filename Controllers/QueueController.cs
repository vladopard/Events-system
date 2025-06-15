using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Events_system.Controllers
{
    [ApiController]
    [Route("api/queues")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _service;
        private readonly IMapper _mapper;

        public QueueController(IQueueService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QueueDTO>>> GetAll()
        {
            var queues = await _service.GetAllAsync();
            return Ok(queues);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<QueueDTO>> Get(int id)
        {
            var queue = await _service.GetByIdAsync(id);
            return Ok(queue);
        }

        [HttpPost]
        public async Task<ActionResult<QueueDTO>> Create([FromBody] QueueCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] QueueUpdateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<QueuePatchDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var existingDto = await _service.GetByIdAsync(id);
            var patchDto = _mapper.Map<QueuePatchDTO>(existingDto);

            patchDoc.ApplyTo(patchDto, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (!TryValidateModel(patchDto))
                return UnprocessableEntity(ModelState);

            await _service.PatchAsync(id, patchDto);
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
