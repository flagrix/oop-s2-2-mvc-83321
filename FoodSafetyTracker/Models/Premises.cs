using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodSafetyTracker.Models
{
    public class Premises
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = "";

        [Required, MaxLength(300)]
        public string Address { get; set; } = "";

        [Required, MaxLength(100)]
        public string Town { get; set; } = "";

        public RiskRating RiskRating { get; set; }

        public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }

    public enum RiskRating
    {
        Low,
        Medium,
        High
    }
}
