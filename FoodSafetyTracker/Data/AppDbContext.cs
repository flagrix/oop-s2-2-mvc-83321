using FoodSafetyTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodSafetyTracker.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Premises> Premises => Set<Premises>();
        public DbSet<Inspection> Inspections => Set<Inspection>();
        public DbSet<FollowUp> FollowUps => Set<FollowUp>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Premises across 3 towns
            builder.Entity<Premises>().HasData(
                new Premises { Id = 1,  Name = "Le Bon Goût",        Address = "1 Main St",      Town = "Cork",    RiskRating = RiskRating.High   },
                new Premises { Id = 2,  Name = "Pizza Palace",       Address = "5 Shop St",      Town = "Dublin",  RiskRating = RiskRating.Medium },
                new Premises { Id = 3,  Name = "Green Bowl Café",    Address = "12 Park Ave",    Town = "Galway",  RiskRating = RiskRating.Low    },
                new Premises { Id = 4,  Name = "The Burger Joint",   Address = "8 High St",      Town = "Cork",    RiskRating = RiskRating.High   },
                new Premises { Id = 5,  Name = "Sushi Express",      Address = "22 Grafton St",  Town = "Dublin",  RiskRating = RiskRating.Medium },
                new Premises { Id = 6,  Name = "Ocean Catch",        Address = "3 Harbour Rd",   Town = "Galway",  RiskRating = RiskRating.High   },
                new Premises { Id = 7,  Name = "Corner Bakery",      Address = "17 Bridge St",   Town = "Cork",    RiskRating = RiskRating.Low    },
                new Premises { Id = 8,  Name = "Noodle House",       Address = "9 Temple Bar",   Town = "Dublin",  RiskRating = RiskRating.Medium },
                new Premises { Id = 9,  Name = "The Kebab King",     Address = "45 Eyre Sq",     Town = "Galway",  RiskRating = RiskRating.High   },
                new Premises { Id = 10, Name = "Sunny Side Up",      Address = "6 Patrick St",   Town = "Cork",    RiskRating = RiskRating.Low    },
                new Premises { Id = 11, Name = "Taco Town",          Address = "33 O'Connell St",Town = "Dublin",  RiskRating = RiskRating.Medium },
                new Premises { Id = 12, Name = "The Vegan Spot",     Address = "2 Quay St",      Town = "Galway",  RiskRating = RiskRating.Low    }
            );

            //  25 Inspections
            var now = new DateTime(2025, 1, 1);
            builder.Entity<Inspection>().HasData(
                new Inspection { Id = 1,  PremisesId = 1,  InspectionDate = now.AddDays(-5),   Score = 42, Outcome = OutcomeResult.Fail, Notes = "Poor hygiene in kitchen" },
                new Inspection { Id = 2,  PremisesId = 1,  InspectionDate = now.AddDays(-60),  Score = 55, Outcome = OutcomeResult.Fail, Notes = "Expired food found" },
                new Inspection { Id = 3,  PremisesId = 2,  InspectionDate = now.AddDays(-3),   Score = 85, Outcome = OutcomeResult.Pass, Notes = "Good overall" },
                new Inspection { Id = 4,  PremisesId = 2,  InspectionDate = now.AddDays(-90),  Score = 70, Outcome = OutcomeResult.Pass, Notes = "Minor issues noted" },
                new Inspection { Id = 5,  PremisesId = 3,  InspectionDate = now.AddDays(-10),  Score = 92, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 6,  PremisesId = 4,  InspectionDate = now.AddDays(-7),   Score = 38, Outcome = OutcomeResult.Fail, Notes = "Pest evidence found" },
                new Inspection { Id = 7,  PremisesId = 4,  InspectionDate = now.AddDays(-120), Score = 60, Outcome = OutcomeResult.Fail, Notes = "Temperature violations" },
                new Inspection { Id = 8,  PremisesId = 5,  InspectionDate = now.AddDays(-2),   Score = 88, Outcome = OutcomeResult.Pass, Notes = "Excellent cleanliness" },
                new Inspection { Id = 9,  PremisesId = 6,  InspectionDate = now.AddDays(-15),  Score = 45, Outcome = OutcomeResult.Fail, Notes = "Storage issues" },
                new Inspection { Id = 10, PremisesId = 6,  InspectionDate = now.AddDays(-200), Score = 50, Outcome = OutcomeResult.Fail, Notes = "Cross-contamination risk" },
                new Inspection { Id = 11, PremisesId = 7,  InspectionDate = now.AddDays(-8),   Score = 95, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 12, PremisesId = 8,  InspectionDate = now.AddDays(-12),  Score = 72, Outcome = OutcomeResult.Pass, Notes = "Good practices" },
                new Inspection { Id = 13, PremisesId = 9,  InspectionDate = now.AddDays(-4),   Score = 33, Outcome = OutcomeResult.Fail, Notes = "Multiple violations" },
                new Inspection { Id = 14, PremisesId = 9,  InspectionDate = now.AddDays(-180), Score = 48, Outcome = OutcomeResult.Fail, Notes = "No hand wash station" },
                new Inspection { Id = 15, PremisesId = 10, InspectionDate = now.AddDays(-6),   Score = 90, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 16, PremisesId = 11, InspectionDate = now.AddDays(-9),   Score = 78, Outcome = OutcomeResult.Pass, Notes = "Satisfactory" },
                new Inspection { Id = 17, PremisesId = 12, InspectionDate = now.AddDays(-11),  Score = 88, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 18, PremisesId = 3,  InspectionDate = now.AddDays(-150), Score = 80, Outcome = OutcomeResult.Pass, Notes = "Good" },
                new Inspection { Id = 19, PremisesId = 5,  InspectionDate = now.AddDays(-100), Score = 65, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 20, PremisesId = 7,  InspectionDate = now.AddDays(-250), Score = 91, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 21, PremisesId = 8,  InspectionDate = now.AddDays(-300), Score = 55, Outcome = OutcomeResult.Fail, Notes = "Old violations" },
                new Inspection { Id = 22, PremisesId = 10, InspectionDate = now.AddDays(-400), Score = 82, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 23, PremisesId = 11, InspectionDate = now.AddDays(-50),  Score = 74, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 24, PremisesId = 12, InspectionDate = now.AddDays(-80),  Score = 85, Outcome = OutcomeResult.Pass, Notes = null },
                new Inspection { Id = 25, PremisesId = 2,  InspectionDate = now.AddDays(-1),   Score = 79, Outcome = OutcomeResult.Pass, Notes = "Recent check" }
            );

            // 10 FollowUps
            builder.Entity<FollowUp>().HasData(
                new FollowUp { Id = 1,  InspectionId = 1,  DueDate = now.AddDays(5),    Status = FollowUpStatus.Open   },
                new FollowUp { Id = 2,  InspectionId = 2,  DueDate = now.AddDays(-30),  Status = FollowUpStatus.Open   }, 
                new FollowUp { Id = 3,  InspectionId = 6,  DueDate = now.AddDays(-10),  Status = FollowUpStatus.Open   },  
                new FollowUp { Id = 4,  InspectionId = 7,  DueDate = now.AddDays(-90),  Status = FollowUpStatus.Open   },
                new FollowUp { Id = 5,  InspectionId = 9,  DueDate = now.AddDays(10),   Status = FollowUpStatus.Open   },
                new FollowUp { Id = 6,  InspectionId = 10, DueDate = now.AddDays(-50),  Status = FollowUpStatus.Open   }, 
                new FollowUp { Id = 7,  InspectionId = 13, DueDate = now.AddDays(7),    Status = FollowUpStatus.Open   },
                new FollowUp { Id = 8,  InspectionId = 14, DueDate = now.AddDays(-5),   Status = FollowUpStatus.Closed, ClosedDate = now.AddDays(-3) },
                new FollowUp { Id = 9,  InspectionId = 2,  DueDate = now.AddDays(-70),  Status = FollowUpStatus.Closed, ClosedDate = now.AddDays(-65) },
                new FollowUp { Id = 10, InspectionId = 21, DueDate = now.AddDays(-200), Status = FollowUpStatus.Closed, ClosedDate = now.AddDays(-190) }
            );
        }
    }
}
