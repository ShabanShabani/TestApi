using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class DeleteCustomerCommand
    {
        public int Id { get; set; }
    }

    public class DeleteCustomerCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCustomerCommand command)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.Id);
            if (customer == null) throw new Exception("Customer not found.");

            _unitOfWork.Customers.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
