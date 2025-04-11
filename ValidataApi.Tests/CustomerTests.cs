using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Domain.Entities;

namespace ValidataApi.Tests
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void CalculateTotalPrice_ShouldSumItemsCorrectly()
        {
            var product = new Product("TestProduct", 10);
            var items = new List<Item> { new Item(product, 2) };
            var order = new Order(DateTime.Now, items);

            Assert.AreEqual(20, order.TotalPrice);
        }
        [Test]
        public void UpdateCustomer_ShouldChangeProperties()
        {
            var customer = new Customer("John", "Doe", "123 St", "12345");
            customer.Update("Jane", "Smith", "456 Rd", "67890");

            Assert.AreEqual("Jane", customer.FirstName);
            Assert.AreEqual("Smith", customer.LastName);
        }

        [Test]
        public void DeleteOrder_ShouldRemoveFromList()
        {
            var customer = new Customer("John", "Doe", "123 St", "12345");
            var order = new Order(DateTime.Now, new List<Item> { new Item(new Product("Item", 10m), 1) });
            customer.AddOrder(order);
            customer.RemoveOrder(order.Id);

            Assert.IsEmpty(customer.Orders);
        }
    }
}
