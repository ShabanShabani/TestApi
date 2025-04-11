using Microsoft.AspNetCore.Mvc;
using ValidataApi.Application.Commands;
using ValidataApi.Application.DTOs;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a new order for a customer.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var handler = new CreateOrderCommandHandler(_unitOfWork);
            var orderId = await handler.Handle(command);
            return CreatedAtAction(nameof(GetOrder), new { id = orderId }, orderId);
        }
        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <param name="command">Updated order details including items.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderCommand command)
        {
            try
            {
                command.Id = id;
                var handler = new UpdateOrderCommandHandler(_unitOfWork);
                await handler.Handle(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an order.
        /// </summary>
        /// <param name="id">The order ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var command = new DeleteOrderCommand { Id = id };
                var handler = new DeleteOrderCommandHandler(_unitOfWork);
                await handler.Handle(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return NotFound();
            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Items = order.Items.Select(i => new ItemDto
                {
                    ProductName = i.Product.Name,
                    ProductPrice = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
            return Ok(orderDto);
        }
    }
}
