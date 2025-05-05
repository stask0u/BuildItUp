using BuildItUp.Data;
using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildItUp.Services.Implementations
{
    public class EngineService : IEngineService
    {
        private readonly ApplicationDbContext _context;

        public EngineService(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<IEnumerable<Engine>> GetAllAsync() =>
            await _context.Engines.Include(e => e.Company).Include(e=>e.Cars).ToListAsync();

        public async Task<Engine?> GetByIdAsync(int id) =>
            await _context.Engines.Include(e => e.Company).FirstOrDefaultAsync(e => e.Id == id);
        public async Task AddAsync(Engine engine)
        {
            try
            {
                var companyExists = await _context.Companies.AnyAsync(c => c.Id == engine.Company.Id);
                if (!companyExists)
                    throw new Exception("Company not found");

                _context.Engines.Add(engine);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding engine: {ex.Message}");
                throw;
            }
        }


        public async Task UpdateAsync(Engine engine)
        {
            _context.Engines.Update(engine);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var engine = await _context.Engines.FindAsync(id);
            if (engine != null)
            {
                _context.Engines.Remove(engine);
                await _context.SaveChangesAsync();
            }
        }
    }
}
