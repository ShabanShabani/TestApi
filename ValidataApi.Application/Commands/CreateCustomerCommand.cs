using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Domain.Entities;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class CreateCustomerCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }

    public class CreateCustomerCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCustomerCommand command)
        {
            var customer = new Customer(command.FirstName, command.LastName, command.Address, command.PostalCode);
            _unitOfWork.Customers.Add(customer);
            await _unitOfWork.SaveChangesAsync();
            return customer.Id;
        }
    }
}
