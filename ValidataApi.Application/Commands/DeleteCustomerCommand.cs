using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class DeleteCustomerCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;

        public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCustomerCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting customer {CustomerId}", command.Id);
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.Id);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {CustomerId}", command.Id);
                throw new KeyNotFoundException("Customer not found.");
            }

            _unitOfWork.Customers.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Customer {CustomerId} deleted", command.Id);
        }
    }
}
