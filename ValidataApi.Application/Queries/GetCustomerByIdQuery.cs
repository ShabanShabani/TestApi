using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.DTOs;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public int Id { get; set; }
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

        public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCustomerByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching customer {CustomerId}", query.Id);
            var customer = await _unitOfWork.Customers.GetByIdAsync(query.Id);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {CustomerId}", query.Id);
                throw new KeyNotFoundException("Customer not found.");
            }

            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PostalCode = customer.PostalCode
            };
        }
    }
}
