using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.DTOs;
using ValidataApi.Domain.Entities;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class CreateOrderCommand
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<ItemDto> Items { get; set; }
    }

    public class CreateOrderCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateOrderCommand command)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.CustomerId);
            if (customer == null) throw new Exception("Customer not found.");

            var items = command.Items.Select(dto => new Item(
                new Product(dto.ProductName, dto.ProductPrice),
                dto.Quantity
            )).ToList();

            var order = new Order(command.OrderDate, items);
            customer.AddOrder(order);
            _unitOfWork.Orders.Add(order);
            await _unitOfWork.SaveChangesAsync();
            return order.Id;
        }
    }
}
