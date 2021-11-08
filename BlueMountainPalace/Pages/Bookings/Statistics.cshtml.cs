using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueMountainPalace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlueMountainPalace.Pages.Bookings
{
    [Authorize(Roles = "Administrator")]
    public class StatisticsModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public StatisticsModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // For passing the results to the Content file
        public IList<CustomerStatistics> CustomerStats { get; set; }
        public IList<RoomBookingStatistics> RoomBookingStats { get; set; }

        // GET: Movies/CalcGenreStats
        public async Task<IActionResult> OnGetAsync()
        {
            // divide the data into groups
            var customerGroups = _context.Customer.GroupBy(c => c.Postcode);
            var bookingGroups = _context.Booking.GroupBy(r => r.RoomID);

            // for each group, get the category value and the measurement
            CustomerStats = await customerGroups.Select(c => new CustomerStatistics { Postcode = c.Key, CustomerCount = c.Count() }).ToListAsync();
            RoomBookingStats = await bookingGroups.Select(r => new RoomBookingStatistics { RoomID = r.Key, BookingCount = r.Count() }).ToListAsync();

            return Page();
        }
    }
}