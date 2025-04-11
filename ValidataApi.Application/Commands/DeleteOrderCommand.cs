using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class DeleteOrderCommand
    {
        public int Id { get; set; }
    }

    public class DeleteOrderCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteOrderCommand command)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(command.Id);
            if (order == null) throw new Exception("Order not found.");

            _unitOfWork.Orders.Delete(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
