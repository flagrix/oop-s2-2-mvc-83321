using FoodSafetyTracker.Data;
using FoodSafetyTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FoodSafetyTracker.Controllers
{
    [Authorize]
    public class PremisesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<PremisesController> _logger;

        public PremisesController(AppDbContext db, ILogger<PremisesController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var premises = await _db.Premises.ToListAsync();
            return View(premises);
        }

        public async Task<IActionResult> Details(int id)
        {
            var premises = await _db.Premises
                .Include(p => p.Inspections)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (premises == null)
            {
                _logger.LogWarning("Premises {Id} not found", id);
                return NotFound();
            }

            return View(premises);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Premises premises)
        {
            if (!ModelState.IsValid) return View(premises);

            _db.Premises.Add(premises);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Premises created: {Name} (Id={Id}) by {User}",
                premises.Name, premises.Id, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var premises = await _db.Premises.FindAsync(id);
            if (premises == null) return NotFound();
            return View(premises);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Premises premises)
        {
            if (id != premises.Id) return BadRequest();
            if (!ModelState.IsValid) return View(premises);

            _db.Premises.Update(premises);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Premises updated: {Name} (Id={Id}) by {User}",
                premises.Name, premises.Id, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var premises = await _db.Premises.FindAsync(id);
            if (premises == null) return NotFound();
            return View(premises);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var premises = await _db.Premises.FindAsync(id);
            if (premises == null) return NotFound();

            _db.Premises.Remove(premises);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Premises deleted: Id={Id} by {User}", id, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}
