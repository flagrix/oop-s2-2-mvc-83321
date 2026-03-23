using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodSafetyTracker.Models
{
    public class Inspection
    {
        public int Id { get; set; }

        [Required]
        public int PremisesId { get; set; }
        public Premises Premises { get; set; } = null!;

        [Required]
        public DateTime InspectionDate { get; set; }

        [Range(0, 100)]
        public int Score { get; set; }

        public OutcomeResult Outcome { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
    }

    public enum OutcomeResult
    {
        Pass,
        Fail
    }
}
