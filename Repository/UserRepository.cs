using Excursion.Data;
using Excursion.Interfaces;
using Excursion.Models;
using Microsoft.EntityFrameworkCore;

namespace Excursion.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsForUser(string userId)
        {
            return await _context.Bookings
                        .Include(b => b.Tour)
                        .Where(b => b.AppUser.Id == userId)
                        .ToListAsync();
        }
        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}