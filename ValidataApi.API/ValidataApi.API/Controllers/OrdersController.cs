using MediatR;
using Microsoft.AspNetCore.Mvc;
using ValidataApi.Application.Commands;
using ValidataApi.Application.DTOs;
using ValidataApi.Application.Queries;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { id = orderId }, orderId);
        }

        /// <summary>
        /// Gets orders for a customer.
        /// </summary>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerOrders(int customerId)
        {
            var query = new GetCustomerOrdersByDateQuery { CustomerId = customerId };
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        /// <summary>
        /// Updates an order.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderCommand command)
        {
            if (!ModelState.IsValid || id != command.Id)
                return BadRequest(ModelState);

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes an order.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        private async Task<IActionResult> GetOrder(int id)
        {
            // Placeholder for GetOrder by ID if needed
            return NotFound();
        }
    }
}
