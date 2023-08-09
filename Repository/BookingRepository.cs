using Excursion.Data;
using Excursion.Interfaces;
using Excursion.Models;
using Microsoft.EntityFrameworkCore;

namespace Excursion.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public bool Add(Booking booking)
        {
            _context.Add(booking);
            return Save();
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            return await _context.Users.FindAsync(curUserId);

        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Delete(Booking booking)
        {
            _context.Remove(booking);
            return Save();
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings.Include(b => b.Tour).FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<bool> RemoveBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Booking> GetByIdAsync(int id)
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