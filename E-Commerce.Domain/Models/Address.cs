using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Models
{
    public class Address : BaseModel
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
