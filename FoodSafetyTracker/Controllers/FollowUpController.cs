using FoodSafetyTracker.Data;
using FoodSafetyTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FoodSafetyTracker.Controllers
{
    [Authorize]
    public class FollowUpController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<FollowUpController> _logger;

        public FollowUpController(AppDbContext db, ILogger<FollowUpController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var followUps = await _db.FollowUps
                .Include(f => f.Inspection)
                    .ThenInclude(i => i.Premises)
                .ToListAsync();

            return View(followUps);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create(int inspectionId)
        {
            var inspection = await _db.Inspections
                .Include(i => i.Premises)
                .FirstOrDefaultAsync(i => i.Id == inspectionId);

            if (inspection == null) return NotFound();

            ViewBag.Inspection = inspection;
            return View(new FollowUp { InspectionId = inspectionId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create(FollowUp followUp)
        {
            // Business rule: DueDate must be after InspectionDate
            var inspection = await _db.Inspections.FindAsync(followUp.InspectionId);
            if (inspection != null && followUp.DueDate < inspection.InspectionDate)
            {
                _logger.LogWarning(
                    "FollowUp creation rejected: DueDate {DueDate} is before InspectionDate {InspectionDate} for InspectionId={InspectionId}",
                    followUp.DueDate, inspection.InspectionDate, followUp.InspectionId);

                ModelState.AddModelError("DueDate", "Due date cannot be before the inspection date.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Inspection = inspection;
                return View(followUp);
            }

            _db.FollowUps.Add(followUp);
            await _db.SaveChangesAsync();

            _logger.LogInformation("FollowUp created: Id={Id}, InspectionId={InspectionId}, DueDate={DueDate} by {User}",
                followUp.Id, followUp.InspectionId, followUp.DueDate, User.Identity?.Name);

            return RedirectToAction("Details", "Inspection", new { id = followUp.InspectionId });
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Close(int id)
        {
            var followUp = await _db.FollowUps
                .Include(f => f.Inspection)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (followUp == null) return NotFound();
            return View(followUp);
        }

        [HttpPost, ActionName("Close"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> CloseConfirmed(int id)
        {
            var followUp = await _db.FollowUps.FindAsync(id);
            if (followUp == null) return NotFound();

            followUp.Status = FollowUpStatus.Closed;
            followUp.ClosedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            _logger.LogInformation("FollowUp closed: Id={Id}, ClosedDate={ClosedDate} by {User}",
                followUp.Id, followUp.ClosedDate, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var followUp = await _db.FollowUps
                .Include(f => f.Inspection)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (followUp == null) return NotFound();
            return View(followUp);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var followUp = await _db.FollowUps.FindAsync(id);
            if (followUp == null) return NotFound();

            _db.FollowUps.Remove(followUp);
            await _db.SaveChangesAsync();

            _logger.LogInformation("FollowUp deleted: Id={Id} by {User}", id, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}
