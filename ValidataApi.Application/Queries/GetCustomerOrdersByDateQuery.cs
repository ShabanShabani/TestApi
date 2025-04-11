using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.DTOs;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Queries
{
    public class GetCustomerOrdersByDateQuery
    {
        public int CustomerId { get; set; }
    }

    public class GetCustomerOrdersByDateQueryHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerOrdersByDateQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetCustomerOrdersByDateQuery query)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(query.CustomerId);
            if (customer == null) return null;

            return customer.Orders
                .OrderBy(o => o.OrderDate)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Items = o.Items.Select(i => new ItemDto
                    {
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity
                    }).ToList()
                });
        }
    }
}
