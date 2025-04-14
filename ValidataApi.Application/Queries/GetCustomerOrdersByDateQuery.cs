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
    public class GetCustomerOrdersByDateQuery : IRequest<IEnumerable<OrderDto>>
    {
        public int CustomerId { get; set; }
    }

    public class GetCustomerOrdersByDateQueryHandler : IRequestHandler<GetCustomerOrdersByDateQuery, IEnumerable<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetCustomerOrdersByDateQueryHandler> _logger;

        public GetCustomerOrdersByDateQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCustomerOrdersByDateQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetCustomerOrdersByDateQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching orders for CustomerId {CustomerId}", query.CustomerId);
            var customer = await _unitOfWork.Customers.GetByIdAsync(query.CustomerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {CustomerId}", query.CustomerId);
                return Enumerable.Empty<OrderDto>();
            }

            var orders = customer.Orders
                .OrderBy(o => o.OrderDate)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Items = o.Items.Select(i => new ItemDto
                    {
                        ProductName = i.Product.Name,
                        ProductPrice = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                });

            _logger.LogInformation("Retrieved {OrderCount} orders for CustomerId {CustomerId}", orders.Count(), query.CustomerId);
            return orders;
        }
    }
}
