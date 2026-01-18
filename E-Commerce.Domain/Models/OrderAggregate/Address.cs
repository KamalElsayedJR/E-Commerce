using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Models.OrderAggregate
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string fname, string lname, string city, string country, string street)
        {
            Fname = fname;
            Lname = lname;
            City = city;
            Country = country;
            Street = street;
        }

        public string Fname { get; set; }
        public string Lname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
    }
}
