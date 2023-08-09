using Excursion.Models;

namespace Excursion.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<IEnumerable<Booking>> GetBookingsForUser(string userId);
        bool Delete(AppUser user);
       
    }
}