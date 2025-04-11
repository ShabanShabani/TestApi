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
    public class UpdateOrderCommand
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<ItemDto> Items { get; set; }
    }

    public class UpdateOrderCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateOrderCommand command)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(command.Id);
            if (order == null) throw new Exception("Order not found.");

            var items = command.Items.Select(i => new Item(
                new Product(i.ProductName, i.ProductPrice), i.Quantity)).ToList();
            order.Update(command.OrderDate, items);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
