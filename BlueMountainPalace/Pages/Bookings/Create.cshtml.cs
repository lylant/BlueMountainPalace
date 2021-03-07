using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BlueMountainPalace.Models;

namespace BlueMountainPalace.Pages.Bookings
{
    [Authorize(Roles = "Administrator")] // Only logged-in admin can access this page
    public class CreateModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public CreateModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CustomerSelection"] = new SelectList(_context.Customer, "Email", "FullName");
            return Page();
        }

        // Prepare IList<Room> for a query to check the booking is available
        public IList<Room> Rooms { get; set; }

        [BindProperty]
        public BookingManageView BookingInput { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Retrieve the Customers list again to display in the web form properly
            ViewData["CustomerSelection"] = new SelectList(_context.Customer, "Email", "FullName");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Need to check whether the submitted booking is available
            // Prepare the query for getting the entire list of Rooms

            // Prepare the parameters to be inserted into the query
            var queryParamRoomID = new SqliteParameter("roomID", BookingInput.RoomID);
            var queryParamCheckIn = new SqliteParameter("checkIn", BookingInput.CheckIn);
            var queryParamCheckOut = new SqliteParameter("checkOut", BookingInput.CheckOut);

            // Construct the query to get available rooms for the search
            var queryBookingRoom = _context.Room.FromSqlRaw("SELECT * FROM Room WHERE Room.ID = @roomID AND Room.ID NOT IN "
                + "(SELECT RoomID FROM Booking WHERE Booking.CheckIn < @checkOut AND Booking.CheckOut > @checkIn);"
                , queryParamRoomID, queryParamCheckIn, queryParamCheckOut);

            Rooms = await queryBookingRoom.ToListAsync();

            // Check the query got the result, if so, the booking is available
            if (Rooms.Count != 0)
            {
                // Create a Booking object for inserting into the database
                Booking booking = new Booking();

                // Construct this Booking object based on BookingInput
                booking.RoomID = BookingInput.RoomID;
                booking.CustomerEmail = BookingInput.CustomerEmail;
                booking.CheckIn = BookingInput.CheckIn;
                booking.CheckOut = BookingInput.CheckOut;
                booking.Cost = BookingInput.Cost;

                // Insert the record into the database, then update the database
                _context.Booking.Add(booking);
                await _context.SaveChangesAsync();

                // Retrieve the customer to find his/her name
                var theCustomer = await _context.Customer.FindAsync(BookingInput.CustomerEmail);
                ViewData["CustomerName"] = theCustomer.FullName;

                ViewData["ValidBooking"] = "true"; // Booking successful
            }
            else
            {
                ViewData["ValidBooking"] = "false"; // Booking failed
            }

            // Invoke the page
            return Page();
        }
    }
}