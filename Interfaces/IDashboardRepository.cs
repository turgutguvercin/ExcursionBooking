using Excursion.Models;

namespace Excursion.Interfaces
{
    public interface IDashboardRepository
    {
         Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
         Task<Booking?> GetBookingByIdAsync(int id);
         Task<bool> RemoveBookingAsync(Booking booking);

    }
}