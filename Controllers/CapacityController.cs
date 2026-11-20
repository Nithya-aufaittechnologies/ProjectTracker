using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MpcFabricsTracker.Data;
using MpcFabricsTracker.Models;

namespace MpcFabricsTracker.Controllers;

public class CapacityController : Controller
{
    private readonly AppDbContext _db;

    public CapacityController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _db.WeeklyCapacities
            .Include(c => c.Week)
            .Include(c => c.Developer)
            .OrderBy(c => c.Week!.WeekNumber)
            .ThenBy(c => c.Developer!.Name)
            .ToListAsync();
        return View(items);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.WeeklyCapacities
            .Include(c => c.Week)
            .Include(c => c.Developer)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, WeeklyCapacity model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _db.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Weekly capacity updated.";
            return RedirectToAction(nameof(Index));
        }

        model.Week = await _db.Weeks.FindAsync(model.WeekId);
        model.Developer = await _db.Developers.FindAsync(model.DeveloperId);
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.WeekId = new SelectList(await _db.Weeks.OrderBy(w => w.WeekNumber).ToListAsync(), "Id", "Label");
        ViewBag.DeveloperId = new SelectList(await _db.Developers.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
        return View(new WeeklyCapacity { PlannedHours = 40 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WeeklyCapacity model)
    {
        var exists = await _db.WeeklyCapacities.AnyAsync(c =>
            c.WeekId == model.WeekId && c.DeveloperId == model.DeveloperId);

        if (exists)
        {
            ModelState.AddModelError(string.Empty, "Capacity already exists for this developer and week.");
        }

        if (ModelState.IsValid)
        {
            _db.WeeklyCapacities.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Weekly capacity created.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.WeekId = new SelectList(await _db.Weeks.OrderBy(w => w.WeekNumber).ToListAsync(), "Id", "Label", model.WeekId);
        ViewBag.DeveloperId = new SelectList(await _db.Developers.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", model.DeveloperId);
        return View(model);
    }
}
