using FoodSafetyTracker.Data;
using FoodSafetyTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FoodSafetyTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(AppDbContext db, ILogger<DashboardController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? town, RiskRating? riskRating)
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var query = _db.Inspections.Include(i => i.Premises).AsQueryable();

            if (!string.IsNullOrEmpty(town))
                query = query.Where(i => i.Premises.Town == town);

            if (riskRating.HasValue)
                query = query.Where(i => i.Premises.RiskRating == riskRating.Value);

            var inspectionsThisMonth = await query
                .CountAsync(i => i.InspectionDate >= startOfMonth);

            var failedThisMonth = await query
                .CountAsync(i => i.InspectionDate >= startOfMonth && i.Outcome == OutcomeResult.Fail);

            var overdueFollowUps = await _db.FollowUps
                .CountAsync(f => f.DueDate < now && f.Status == FollowUpStatus.Open);

            var towns = await _db.Premises
                .Select(p => p.Town)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            _logger.LogInformation(
                "Dashboard loaded by {User}. InspectionsThisMonth={Count}, Failed={Failed}, Overdue={Overdue}, Town={Town}, Risk={Risk}",
                User.Identity?.Name, inspectionsThisMonth, failedThisMonth, overdueFollowUps, town, riskRating);

            var vm = new DashboardViewModel
            {
                InspectionsThisMonth = inspectionsThisMonth,
                FailedThisMonth = failedThisMonth,
                OverdueFollowUps = overdueFollowUps,
                SelectedTown = town,
                SelectedRiskRating = riskRating,
                Towns = towns
            };

            return View(vm);
        }
    }
}
