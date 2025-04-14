using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.DTOs;
using ValidataApi.Domain.Entities;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class UpdateCustomerCommand : IRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        public string Address { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;

        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCustomerCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating customer {CustomerId}", command.Id);
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.Id);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {CustomerId}", command.Id);
                throw new KeyNotFoundException("Customer not found.");
            }

            customer.Update(command.FirstName, command.LastName, command.Address, command.PostalCode);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Customer {CustomerId} updated", command.Id);
        }
    }
}
