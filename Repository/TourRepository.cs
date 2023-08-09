using Excursion.Data;
using Excursion.Interfaces;
using Excursion.Models;
using Microsoft.EntityFrameworkCore;

namespace Excursion.Repository
{
    public class TourRepository : ITourRepository
    {
        private readonly ApplicationDbContext _context;

        public TourRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public bool Add(Tour tour)
        {
            _context.Add(tour);
            return Save();
        }

        public bool Delete(Tour tour)
        {
            _context.Remove(tour);
            return Save();
        }

        public async Task<IEnumerable<Tour>> GetAll()
        {
            return await _context.Tours.ToListAsync();
        }

        public async Task<Tour?> GetByIdAsync(int id)
        {
            return await _context.Tours.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Tour>> GetTourByCity(string city)
        {
            return await _context.Tours.Include(i => i.Address).Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Tour tour)
        {
            _context.Update(tour);
            return Save();
        }

        public async Task<IEnumerable<Tour>> SearchToursAsync(string searchTerm)
        {
            return await _context.Tours
                .Where(t => t.Name.Contains(searchTerm) || t.Description.Contains(searchTerm))
                .ToListAsync();
        }
    }
}