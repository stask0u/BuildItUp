using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BuildItUp.Models.Entities
{
    public class Engine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EngineModel { get; set; } = string.Empty;

        [Required]
        public int Horsepower { get; set; }

        public int CompanyId { get; set; }  
        public Company Company { get; set; } = null!;

        public ICollection<Car> Cars { get; set; } = new List<Car>();

    }
}
