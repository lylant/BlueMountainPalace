using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string Level { get; set; }
        public int BedCount { get; set; }
        public decimal Price { get; set; }
        public ICollection<Booking> TheBookings { get; set; }
    }
}
