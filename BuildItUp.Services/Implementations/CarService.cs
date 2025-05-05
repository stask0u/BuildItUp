using BuildItUp.Data;
using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildItUp.Services.Implementations
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllAsync() =>
            await _context.Cars.Include(c => c.Engine).ThenInclude(e => e.Company).ToListAsync();

        public async Task<Car?> GetByIdAsync(int id) =>
            await _context.Cars.Include(c => c.Engine).ThenInclude(e => e.Company).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> CarExistsAsync(int id) =>
            await _context.Cars.AnyAsync(c => c.Id == id);
    
    }
}
