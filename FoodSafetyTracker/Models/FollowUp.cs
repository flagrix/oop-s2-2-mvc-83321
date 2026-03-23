using System;
using System.ComponentModel.DataAnnotations;

namespace FoodSafetyTracker.Models
{
    public class FollowUp
    {
        public int Id { get; set; }

        [Required]
        public int InspectionId { get; set; }
        public Inspection Inspection { get; set; } = null!;

        [Required]
        public DateTime DueDate { get; set; }

        public FollowUpStatus Status { get; set; }

        public DateTime? ClosedDate { get; set; }
    }

    public enum FollowUpStatus
    {
        Open,
        Closed
    }
}
