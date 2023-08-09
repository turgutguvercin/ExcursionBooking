using Excursion.Models;

namespace Excursion.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<AppUser> GetCurrentUserAsync();
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<bool> RemoveBookingAsync(Booking booking);
        bool Add(Booking booking);
        bool Update(AppUser user);
        bool Delete(Booking booking);
        bool Save();
    }
}