using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidataApi.Domain.Entities
{
    public class Item
    {
        public int Id { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public int OrderId { get; private set; }
        public Item() { }


        public Item(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
