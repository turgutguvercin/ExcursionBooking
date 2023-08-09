using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Excursion.Models;
using Microsoft.EntityFrameworkCore;

namespace Excursion.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Tour>  Tours { get; set; }
    }
}