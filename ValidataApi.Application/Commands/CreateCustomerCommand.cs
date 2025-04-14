using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Domain.Entities;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class CreateCustomerCommand : IRequest<int>
    {
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

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCustomerCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating customer {FirstName} {LastName}", command.FirstName, command.LastName);
            var customer = new Customer(command.FirstName, command.LastName, command.Address, command.PostalCode);
            _unitOfWork.Customers.Add(customer);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Customer created with Id {CustomerId}", customer.Id);
            return customer.Id;
        }
    }
}
