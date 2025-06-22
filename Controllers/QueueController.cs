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

        public QueueController(IQueueService service) => _service = service;

        // GET /api/queues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QueueDTO>>> GetAll()
            => Ok(await _service.GetAllAsync());

        // GET /api/queues/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<QueueDTO>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        // GET /api/queues/waiting
        [HttpGet("waiting")]
        public async Task<ActionResult<IEnumerable<QueueDTO>>> GetWaiting()
            => Ok(await _service.GetWaitingAsync());

        // GET /api/queues/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<QueueDTO>>> GetByUser(string userId)
            => Ok(await _service.GetByUserIdAsync(userId));

        // PUT /api/queues/{id}/notify
        [HttpPut("{id:int}/notify")]
        public async Task<IActionResult> Notify(int id)
        {
            await _service.NotifyAsync(id);
            return NoContent();
        }


        // DELETE /api/queues/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
