using Microsoft.AspNetCore.Mvc;
using ValidataApi.Application.Commands;
using ValidataApi.Application.Queries;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var handler = new CreateCustomerCommandHandler(_unitOfWork);
            var customerId = await handler.Handle(command);
            return Ok(customerId);
        }

        /// <summary>
        /// Gets a customer's orders sorted by date.
        /// </summary>
        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetOrders(int id)
        {
            var query = new GetCustomerOrdersByDateQuery { CustomerId = id };
            var handler = new GetCustomerOrdersByDateQueryHandler(_unitOfWork);
            var orders = await handler.Handle(query);
            return Ok(orders);
        }
    }
}
