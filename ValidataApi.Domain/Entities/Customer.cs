using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidataApi.Domain.Entities
{
    public class Customer
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string PostalCode { get; private set; }
        public List<Order> Orders { get; private set; } = new List<Order>();

        public Customer(string firstName, string lastName, string address, string postalCode)
        {
            ValidateName(firstName, lastName);
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PostalCode = postalCode;
        }

        public void Update(string firstName, string lastName, string address, string postalCode)
        {
            ValidateName(firstName, lastName);
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PostalCode = postalCode;
        }

        public void AddOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            Orders.Add(order);
        }

        public void RemoveOrder(int orderId)
        {
            var order = Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null) Orders.Remove(order);
        }

        private void ValidateName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("FirstName and LastName cannot be empty.");
        }
    }
}
