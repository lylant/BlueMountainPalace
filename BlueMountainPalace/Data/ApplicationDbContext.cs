using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlueMountainPalace.Models;

namespace BlueMountainPalace.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BlueMountainPalace.Models.Booking> Booking { get; set; }
        public DbSet<BlueMountainPalace.Models.Customer> Customer { get; set; }
        public DbSet<BlueMountainPalace.Models.Room> Room { get; set; }
    }
}
