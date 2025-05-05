using BuildItUp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildItUp.Services.Interfaces
{
    public interface IEngineService
    {
        Task<IEnumerable<Engine>> GetAllAsync();
        Task<Engine?> GetByIdAsync(int id);
        Task AddAsync(Engine engine);
        Task UpdateAsync(Engine engine);
        Task DeleteAsync(int id);
    }
}
