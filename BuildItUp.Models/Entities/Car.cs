using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildItUp.Models.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        public int EngineId { get; set; }
        public Engine Engine { get; set; } = null!;
        public int CompanyId {  get; set; }
        public Company? Company { get; set; } = null!;
    }
}
