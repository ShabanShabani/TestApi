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
    public class UpdateCustomerCommand
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }

    public class UpdateCustomerCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCustomerCommand command)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.Id);
            if (customer == null) throw new Exception("Customer not found.");

            customer.Update(command.FirstName, command.LastName, command.Address, command.PostalCode);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
