using AutoMapper;
using Events_system.BusinessServices;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Events_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;

        public OrderController(IOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> Get(int id)
        {
            var order = await _service.GetByIdAsync(id);
            return Ok(order);
        }

        [HttpPost("order-or-queue")]
        public async Task<ActionResult<OrderOrQueueResponseDTO>> Createe([FromBody] OrderRequestDTO dto)
        {
            var result = await _service.CreateeAsync(dto);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Create(OrderCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, OrderUpdateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<OrderPatchDTO> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var orderDto = await _service.GetByIdAsync(id);
            var orderForPatch = _mapper.Map<OrderPatchDTO>(orderDto);

            patchDoc.ApplyTo(orderForPatch, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (!TryValidateModel(orderForPatch))
                return UnprocessableEntity(ModelState);

            await _service.PatchAsync(id, orderForPatch);
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

