using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BlueMountainPalace.Data;
using BlueMountainPalace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlueMountainPalace.Pages.Bookings
{
    [Authorize(Roles = "Customer")] // Only logged-in customer can access this page
    public class BookingModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public BookingModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["RoomSelection"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        // Prepare IList<Room> for a query to check the booking is available
        public IList<Room> Rooms { get; set; }

        [BindProperty]
        public BookingView BookingInput { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            // Retrieve the RoomID list again to display in the web form properly
            ViewData["RoomSelection"] = new SelectList(_context.Room, "ID", "ID");

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

                // Retrieve the logged-in user's email
                string _email = User.FindFirst(ClaimTypes.Name).Value;

                // Construct this Booking object based on BookingInput
                booking.RoomID = BookingInput.RoomID;
                booking.CustomerEmail = _email; // The customer should be the logged-in user
                booking.CheckIn = BookingInput.CheckIn;
                booking.CheckOut = BookingInput.CheckOut;

                // Retrieve the ordered room to evaluate the amount due
                var theRoom = await _context.Room.FindAsync(BookingInput.RoomID);

                // Evaluate the amount due of this booking
                booking.Cost = (BookingInput.CheckOut - BookingInput.CheckIn).Days * theRoom.Price;

                // Pass some data to content file
                ViewData["AmountDue"] = booking.Cost;
                ViewData["RoomLevel"] = theRoom.Level;

                // Insert the record into the database, then update the database
                _context.Booking.Add(booking);
                await _context.SaveChangesAsync();

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