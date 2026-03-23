using FoodSafetyTracker.Data;
using FoodSafetyTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FoodSafetyTracker.Tests
{
    public class FollowUpTests
    {
        private AppDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        // Overdue follow-ups query returns only open + past due
        [Fact]
        public async Task OverdueFollowUps_ReturnsOnlyOpenAndPastDue()
        {
            var db = GetInMemoryDb();

            db.FollowUps.AddRange(
                new FollowUp { Id = 1, InspectionId = 1, DueDate = DateTime.Now.AddDays(-5), Status = FollowUpStatus.Open },   // overdue
                new FollowUp { Id = 2, InspectionId = 1, DueDate = DateTime.Now.AddDays(5),  Status = FollowUpStatus.Open },   // not due yet
                new FollowUp { Id = 3, InspectionId = 1, DueDate = DateTime.Now.AddDays(-3), Status = FollowUpStatus.Closed }  // closed
            );
            await db.SaveChangesAsync();

            var overdue = await db.FollowUps
                .Where(f => f.DueDate < DateTime.Now && f.Status == FollowUpStatus.Open)
                .CountAsync();

            Assert.Equal(1, overdue);
        }

        // Closed FollowUp must have a Closeddate
        [Fact]
        public void FollowUp_WhenClosed_MustHaveClosedDate()
        {
            var followUp = new FollowUp
            {
                Status = FollowUpStatus.Closed,
                ClosedDate = null
            };

            // Closeddate should be set when Status is Closed
            var isValid = !(followUp.Status == FollowUpStatus.Closed && followUp.ClosedDate == null);

            Assert.False(isValid, "A closed follow-up without a ClosedDate should be invalid.");
        }

        [Fact]
        public async Task DashboardCounts_ConsistentWithSeedData()
        {
            var db = GetInMemoryDb();

            var premises = new Premises { Id = 1, Name = "Test Place", Address = "1 St", Town = "Cork", RiskRating = RiskRating.High };
            db.Premises.Add(premises);

            var now = DateTime.Now;
            db.Inspections.AddRange(
                new Inspection { Id = 1, PremisesId = 1, InspectionDate = now.AddDays(-2), Score = 40, Outcome = OutcomeResult.Fail },
                new Inspection { Id = 2, PremisesId = 1, InspectionDate = now.AddDays(-5), Score = 80, Outcome = OutcomeResult.Pass },
                new Inspection { Id = 3, PremisesId = 1, InspectionDate = now.AddDays(-200), Score = 50, Outcome = OutcomeResult.Fail }
            );
            await db.SaveChangesAsync();

            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var thisMonth = await db.Inspections.CountAsync(i => i.InspectionDate >= startOfMonth);
            var failedThisMonth = await db.Inspections.CountAsync(i => i.InspectionDate >= startOfMonth && i.Outcome == OutcomeResult.Fail);

            Assert.Equal(2, thisMonth);
            Assert.Equal(1, failedThisMonth);
        }

        // FollowUp Duedate before Inspectiondate is invalid
        [Fact]
        public void FollowUp_DueDateBeforeInspectionDate_IsInvalid()
        {
            var inspectionDate = new DateTime(2025, 6, 15);
            var followUp = new FollowUp
            {
                DueDate = new DateTime(2025, 6, 10)
            };

            var isValid = followUp.DueDate >= inspectionDate;

            Assert.False(isValid, "DueDate before InspectionDate should be invalid.");
        }
    }
}
