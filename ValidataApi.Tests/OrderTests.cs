using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.Commands;
using ValidataApi.Application.DTOs;
using ValidataApi.Domain.Entities;

namespace ValidataApi.Tests
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void CreateOrder_WithItemDto_ShouldCalculateTotalPrice()
        {
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<ItemDto>
            {
                new ItemDto { ProductName = "Laptop", ProductPrice = 100, Quantity = 2 },
                new ItemDto { ProductName = "Mouse", ProductPrice = 50, Quantity = 1 }
            }
            };

            var items = command.Items.Select(dto => new Item(
                new Product(dto.ProductName, dto.ProductPrice),
                dto.Quantity
            )).ToList();

            var order = new Order(command.OrderDate, items);

            Assert.AreEqual(250, order.TotalPrice); 
        }

        [Test]
        public void UpdateOrder_WithItemDto_ShouldUpdateItems()
        {
            var originalItems = new List<Item> { new Item(new Product("Laptop", 100), 1) };
            var order = new Order(DateTime.Now, originalItems);

            var command = new UpdateOrderCommand
            {
                Id = 1,
                OrderDate = DateTime.Now.AddDays(1),
                Items = new List<ItemDto>
            {
                new ItemDto { ProductName = "Tablet", ProductPrice = 200, Quantity = 3 }
            }
            };

            var updatedItems = command.Items.Select(dto => new Item(
                new Product(dto.ProductName, dto.ProductPrice),
                dto.Quantity
            )).ToList();

            order.Update(command.OrderDate, updatedItems);

            Assert.AreEqual(600, order.TotalPrice); // 3*200 = 600
            Assert.AreEqual("Tablet", order.Items.First().Product.Name);
        }

        [Test]
        public void GetCustomerOrdersByDate_ShouldMapToItemDto()
        {
            var customer = new Customer("John", "Doe", "123 St", "12345");
            var items = new List<Item> { new Item(new Product("Laptop", 100), 2) };
            var order = new Order(DateTime.Now, items);
            customer.AddOrder(order);

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
                }).ToList();

            var itemDto = orders.First().Items.First();
            Assert.AreEqual("Laptop", itemDto.ProductName);
            Assert.AreEqual(100, itemDto.ProductPrice);
            Assert.AreEqual(2, itemDto.Quantity);
        }
        [Test]
        public void CreateOrder_WithItemDto_ShouldSetCorrectTotalPrice()
        {
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<ItemDto>
            {
                new ItemDto { ProductName = "Laptop", ProductPrice = 100, Quantity = 2 },
                new ItemDto { ProductName = "Mouse", ProductPrice = 50, Quantity = 1 }
            }
            };

            var customer = new Customer("John", "Doe", "123 St", "12345");
            var items = command.Items.Select(dto => new Item(
                new Product(dto.ProductName, dto.ProductPrice),
                dto.Quantity
            )).ToList();

            var order = new Order(command.OrderDate, items);

            Assert.AreEqual(250, order.TotalPrice); 
        }
    }
}
