using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueMountainPalace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BlueMountainPalace.Pages.Rooms
{
    [Authorize(Roles = "Customer")] // Only logged-in customer can access this page
    public class SearchModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public SearchModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // List of available rooms; for passing to content file to display
        public IList<Room> Rooms { get; set; }

        [BindProperty(SupportsGet = true)]
        public SearchView SearchInput { get; set; }


        public IActionResult OnGet()
        {
            // Prepare SelectList for BedCounts
            // Distinct in LINQ will remove duplicated options
            ViewData["BedSelection"] = new SelectList(_context.Room.Select(m => m.BedCount).Distinct());
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Prepare SelectList for BedCounts
            // Distinct in LINQ will remove duplicated options
            ViewData["BedSelection"] = new SelectList(_context.Room.Select(m => m.BedCount).Distinct());


            // Prepare the query for getting the entire list of Rooms

            // Prepare the parameters to be inserted into the query
            var queryParamBedCount = new SqliteParameter("bedCount", SearchInput.BedCount);
            var queryParamCheckIn = new SqliteParameter("checkIn", SearchInput.CheckIn);
            var queryParamCheckOut = new SqliteParameter("checkOut", SearchInput.CheckOut);

            // Construct the query to get available rooms for the search
            var querySearchRoom = _context.Room.FromSqlRaw("SELECT Room.ID, Room.Level, Room.BedCount, Room.Price "
                + "FROM Room WHERE Room.BedCount = @bedCount AND Room.ID NOT IN "
                + "(SELECT RoomID FROM Booking WHERE Booking.CheckIn < @checkOut AND Booking.CheckOut > @checkIn);"
                , queryParamBedCount, queryParamCheckIn, queryParamCheckOut);

            // Run the query and save the results in Rooms for passing to content file
            Rooms = await querySearchRoom.ToListAsync();

            // Set the result exist flag
            // "true": result found, "false": no result, null: no POST request yet
            if (Rooms.Count != 0)
            {
                ViewData["ValidResult"] = "true";
            }
            else
            {
                ViewData["ValidResult"] = "false";
            }

            // Invoke the page
            return Page();
        }
    }
}