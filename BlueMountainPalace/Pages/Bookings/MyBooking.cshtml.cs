using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueMountainPalace.Data;
using BlueMountainPalace.Models;

namespace BlueMountainPalace.Pages.Bookings
{
    [Authorize(Roles = "Customer")] // Only logged-in customer can access this page
    public class MyBookingModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public MyBookingModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get; set; }

        // GET: Bookings/MyBooking/?sortOrder=xxx
        public async Task<IActionResult> OnGetAsync(string sortOrder)
        {
            // Retrieve the logged-in user's email
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            // No parameter is given from the URL
            if (String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "checkin_asc"; // A default sort
            }

            // Prepare the query for getting the entire list of Purchases
            // Convert the datatype from DbSet<Booking> to IQueryable<Booking>
            var bookings = (IQueryable<Booking>)_context.Booking;

            // Add the navigation properties
            // Select the logged-in user's Purchases only
            bookings = bookings
                .Include(p => p.TheCustomer)
                .Include(p => p.TheRoom)
                .Where(p => p.CustomerEmail == _email);

            // Sort the Purchases by specified order
            switch (sortOrder)
            {
                case "checkin_asc":
                    bookings = bookings.OrderBy(p => p.CheckIn);
                    break;
                case "checkin_desc":
                    bookings = bookings.OrderByDescending(p => p.CheckIn);
                    break;
                case "cost_asc":
                    bookings = bookings.OrderBy(p => (double)p.Cost);
                    break;
                case "cost_desc":
                    bookings = bookings.OrderByDescending(p => (double)p.Cost);
                    break;
            }

            // Toggle feature for the sorting order
            ViewData["NextCheckinOrder"] = sortOrder != "checkin_asc" ? "checkin_asc" : "checkin_desc";
            ViewData["NextCostOrder"] = sortOrder != "cost_asc" ? "cost_asc" : "cost_desc";

            // Access the database to execute the query
            // Assign the returned Purchase list to the Purchase property of this PageModel class

            Booking = await bookings.AsNoTracking().ToListAsync();

            return Page();
        }
    }
}