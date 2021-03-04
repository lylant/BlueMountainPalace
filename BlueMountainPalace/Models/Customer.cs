using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class Customer
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Postcode { get; set; }
        public ICollection<Booking> TheBookings { get; set; }
    }
}
