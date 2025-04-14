using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidataApi.Application.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        public string Address { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
