using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidataApi.Application.DTOs
{
    public class ItemDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 100 characters.")]
        public string ProductName { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be positive.")]
        public decimal ProductPrice { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
