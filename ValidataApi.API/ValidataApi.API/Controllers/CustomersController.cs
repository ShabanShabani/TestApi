using MediatR;
using Microsoft.AspNetCore.Mvc;
using ValidataApi.Application.Commands;
using ValidataApi.Application.DTOs;
using ValidataApi.Application.Queries;
using ValidataApi.Domain.Entities;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerId = await _mediator.Send(command);
            var dto = new CustomerDto
            {
                Id = customerId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Address = command.Address,
                PostalCode = command.PostalCode
            };
            return CreatedAtAction(nameof(GetCustomer), new { id = customerId }, dto);
        }

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var query = new GetCustomerByIdQuery { Id = id };
            var customer = await _mediator.Send(query);
            return Ok(customer);
        }

        /// <summary>
        /// Updates a customer.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerCommand command)
        {
            if (!ModelState.IsValid || id != command.Id)
                return BadRequest(ModelState);

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var command = new DeleteCustomerCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
