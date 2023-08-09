using Excursion.Models;

namespace Excursion.Interfaces
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAll();
        Task<Tour?> GetByIdAsync(int id);
        Task<IEnumerable<Tour>> GetTourByCity(string city);
        Task<IEnumerable<Tour>> SearchToursAsync(string searchTerm);
        bool Add(Tour tour);
        bool Update(Tour tour);
        bool Delete(Tour tour);
        bool Save();
    }
}