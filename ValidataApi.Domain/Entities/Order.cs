using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidataApi.Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalPrice { get; private set; }
        public List<Item> Items { get; private set; } = new List<Item>();
        public int CustomerId { get; private set; }

        public Order()
        {
            
        }
        public Order(DateTime orderDate, List<Item> items)
        {
            if (!items.Any()) throw new ArgumentException("Order must have at least one item.");
            OrderDate = orderDate;
            Items = items;
            CalculateTotalPrice();
        }

        public void Update(DateTime orderDate, List<Item> items)
        {
            if (!items.Any()) throw new ArgumentException("Order must have at least one item.");
            OrderDate = orderDate;
            Items = items;
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Items.Sum(item => item.Product.Price * item.Quantity);
        }
    }
}
