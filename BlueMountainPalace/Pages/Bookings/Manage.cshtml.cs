using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueMountainPalace.Models;

namespace BlueMountainPalace.Pages.Bookings
{
    [Authorize(Roles = "Administrator")] // Only logged-in admin can access this page
    public class ManageModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public ManageModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get; set; }

        public async Task OnGetAsync()
        {
            Booking = await _context.Booking
                .Include(b => b.TheCustomer)
                .Include(b => b.TheRoom).ToListAsync();
        }

    }
}