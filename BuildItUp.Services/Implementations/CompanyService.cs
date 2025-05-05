using BuildItUp.Data;
using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildItUp.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllAsync() =>
            await _context.Companies
                .Include(c => c.Engines)  
                .Include(c => c.Cars)
                .ToListAsync();

        public async Task<Company?> GetByIdAsync(int id) =>
            await _context.Companies
                .Include(c => c.Engines)  
                .Include(c => c.Cars)     
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }
    }
}
