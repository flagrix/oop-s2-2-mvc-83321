using FoodSafetyTracker.Data;
using FoodSafetyTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FoodSafetyTracker.Controllers
{
    [Authorize]
    public class InspectionController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<InspectionController> _logger;

        public InspectionController(AppDbContext db, ILogger<InspectionController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var inspections = await _db.Inspections
                .Include(i => i.Premises)
                .OrderByDescending(i => i.InspectionDate)
                .ToListAsync();

            return View(inspections);
        }

        public async Task<IActionResult> Details(int id)
        {
            var inspection = await _db.Inspections
                .Include(i => i.Premises)
                .Include(i => i.FollowUps)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inspection == null)
            {
                _logger.LogWarning("Inspection {Id} not found", id);
                return NotFound();
            }

            return View(inspection);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Premises = new SelectList(await _db.Premises.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create(Inspection inspection)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Premises = new SelectList(await _db.Premises.ToListAsync(), "Id", "Name");
                return View(inspection);
            }

            _db.Inspections.Add(inspection);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Inspection created: Id={Id}, PremisesId={PremisesId}, Outcome={Outcome} by {User}",
                inspection.Id, inspection.PremisesId, inspection.Outcome, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int id)
        {
            var inspection = await _db.Inspections.FindAsync(id);
            if (inspection == null) return NotFound();

            ViewBag.Premises = new SelectList(await _db.Premises.ToListAsync(), "Id", "Name", inspection.PremisesId);
            return View(inspection);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int id, Inspection inspection)
        {
            if (id != inspection.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Premises = new SelectList(await _db.Premises.ToListAsync(), "Id", "Name", inspection.PremisesId);
                return View(inspection);
            }

            _db.Inspections.Update(inspection);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Inspection updated: Id={Id}, PremisesId={PremisesId} by {User}",
                inspection.Id, inspection.PremisesId, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var inspection = await _db.Inspections
                .Include(i => i.Premises)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inspection == null) return NotFound();
            return View(inspection);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inspection = await _db.Inspections.FindAsync(id);
            if (inspection == null) return NotFound();

            _db.Inspections.Remove(inspection);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Inspection deleted: Id={Id} by {User}", id, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}
