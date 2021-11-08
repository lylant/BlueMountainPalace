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
using BlueMountainPalace.Data;
using BlueMountainPalace.Models;

namespace BlueMountainPalace.Pages.Bookings
{
    [Authorize(Roles = "Administrator")] // Only logged-in admin can access this page
    public class EditModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public EditModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Booking
                .Include(b => b.TheCustomer)
                .Include(b => b.TheRoom).FirstOrDefaultAsync(m => m.ID == id);

            if (Booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerSelection"] = new SelectList(_context.Customer, "Email", "FullName");
            return Page();
        }


        // Prepare IList<Room> for a query to check the booking is available
        public IList<Room> Rooms { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            // Retrieve the Customers list again to display in the web form properly (if edit is failed)
            ViewData["CustomerSelection"] = new SelectList(_context.Customer, "Email", "FullName");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Need to check whether the submitted editing is available
            // Prepare the query for getting the entire list of Rooms
            // Note: this query should remove the booking itself (currently editing)

            // Prepare the parameters to be inserted into the query
            var queryParamBookingID = new SqliteParameter("bookingID", Booking.ID);
            var queryParamRoomID = new SqliteParameter("roomID", Booking.RoomID);
            var queryParamCheckIn = new SqliteParameter("checkIn", Booking.CheckIn);
            var queryParamCheckOut = new SqliteParameter("checkOut", Booking.CheckOut);

            // Construct the query to get available rooms for the search
            var queryBookingRoom = _context.Room.FromSqlRaw("SELECT * FROM Room WHERE Room.ID = @roomID AND Room.ID NOT IN "
                + "(SELECT RoomID FROM Booking WHERE Booking.ID != @bookingID AND Booking.CheckIn < @checkOut AND Booking.CheckOut > @checkIn);"
                , queryParamBookingID, queryParamRoomID, queryParamCheckIn, queryParamCheckOut);

            Rooms = await queryBookingRoom.ToListAsync();

            // Check the query got the result, if so, the booking is available
            if (Rooms.Count != 0)
            {
                _context.Attach(Booking).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(Booking.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("./Manage");
            }
            else
            {
                ViewData["ValidBooking"] = "false"; // Booking failed
            }

            return Page();
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.ID == id);
        }
    }
}