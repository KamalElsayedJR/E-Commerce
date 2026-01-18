using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.User
{
    public class AddressDto
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
    }
}
