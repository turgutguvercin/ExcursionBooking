using Excursion.Data;
using Excursion.Interfaces;
using Excursion.Models;
using Microsoft.EntityFrameworkCore;

namespace Excursion.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
           return await _context.Bookings
                        .Include(b => b.Tour)
                        .Where(b => b.AppUser.Id == userId)
                        .ToListAsync();
        }
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.AppUser)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> RemoveBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

    }

}