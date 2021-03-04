using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int RoomID { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Cost { get; set; }
        public Room TheRoom { get; set; }
        public Customer TheCustomer { get; set; }
    }
}
