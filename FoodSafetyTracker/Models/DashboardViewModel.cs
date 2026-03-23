using System.Collections.Generic;

namespace FoodSafetyTracker.Models
{
    public class DashboardViewModel
    {
        public int InspectionsThisMonth { get; set; }
        public int FailedThisMonth { get; set; }
        public int OverdueFollowUps { get; set; }

        public string? SelectedTown { get; set; }
        public RiskRating? SelectedRiskRating { get; set; }

        public List<string> Towns { get; set; } = new();
    }
}
