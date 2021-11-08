using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueMountainPalace.Data;
using BlueMountainPalace.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlueMountainPalace.Pages.Customers
{
    [Authorize(Roles = "Customer")] // Only logged-in customer can access this page
    public class MyDetailsModel : PageModel
    {
        private readonly BlueMountainPalace.Data.ApplicationDbContext _context;

        public MyDetailsModel(BlueMountainPalace.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerView Myself { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the logged-in user's email
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer customer = await _context.Customer.FirstOrDefaultAsync(h => h.Email == _email);

            // Check if the details for the user exists in the database
            if (customer != null)
            {
                ViewData["ExistInDB"] = "true"; // Flag as Exist
                Myself = new CustomerView
                {
                    // Retrieve his/her details for display in the web form
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Postcode = customer.Postcode
                };
            }
            else
            {
                ViewData["ExistInDB"] = "false"; // Flag as Non-Exist
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // retrieve the logged-in user's email
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer customer = await _context.Customer.FirstOrDefaultAsync(h => h.Email == _email);

            // Check if the details for the user exists in the database
            if (customer != null)
            {
                // This ViewData entry is needed in the content file
                ViewData["ExistInDB"] = "true";
            }
            else
            {
                ViewData["ExistInDB"] = "false";
            }

            // Check the validity of the values with the data annotations
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (customer == null)
            {
                // If the details do not exist, create a customer object for inserting database
                // Otherwise, customer object is already bound in customer
                customer = new Customer();
            }

            // Construct this moviegoer object based on 'Myself'
            customer.Email = _email;
            customer.FirstName = Myself.FirstName;
            customer.LastName = Myself.LastName;
            customer.Postcode = Myself.Postcode;

            if ((string)ViewData["ExistInDB"] == "true")
            {
                _context.Attach(customer).State = EntityState.Modified;
            }
            else
            {
                _context.Customer.Add(customer);
            }

            try  // catching the conflict of editing this record concurrently
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["SuccessDB"] = "success";
            return Page();
        }
    }
}