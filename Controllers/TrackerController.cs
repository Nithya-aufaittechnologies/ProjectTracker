using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MpcFabricsTracker.Data;
using MpcFabricsTracker.Models;
using MpcFabricsTracker.Models.ViewModels;

namespace MpcFabricsTracker.Controllers;

public class TrackerController : Controller
{
    private readonly AppDbContext _db;

    public TrackerController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(int? weekNumber, int? developerId)
    {
        var query = _db.TrackerEntries
            .Include(t => t.Week)
            .Include(t => t.Developer)
            .AsQueryable();

        if (weekNumber.HasValue)
        {
            query = query.Where(t => t.Week!.WeekNumber == weekNumber.Value);
        }

        if (developerId.HasValue)
        {
            query = query.Where(t => t.DeveloperId == developerId.Value);
        }

        var model = new TrackerIndexViewModel
        {
            WeekNumber = weekNumber,
            DeveloperId = developerId,
            Weeks = new SelectList(await _db.Weeks.OrderBy(w => w.WeekNumber).ToListAsync(), "WeekNumber", "Label", weekNumber),
            Developers = new SelectList(await _db.Developers.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", developerId),
            Entries = await query
                .OrderBy(t => t.Week!.WeekNumber)
                .ThenBy(t => t.WorkDate)
                .ThenBy(t => t.Developer!.Name)
                .ToListAsync()
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var entry = await _db.TrackerEntries
            .Include(t => t.Week)
            .Include(t => t.Developer)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (entry == null)
        {
            return NotFound();
        }

        return View(entry);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateLookups();
        return View(new TrackerEntry
        {
            WorkDate = DateTime.Today,
            PercentCompleted = 0
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrackerEntry entry)
    {
        if (ModelState.IsValid)
        {
            _db.TrackerEntries.Add(entry);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Tracker entry added.";
            return RedirectToAction(nameof(Index));
        }

        await PopulateLookups(entry.WeekId, entry.DeveloperId);
        return View(entry);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entry = await _db.TrackerEntries.FindAsync(id);
        if (entry == null)
        {
            return NotFound();
        }

        await PopulateLookups(entry.WeekId, entry.DeveloperId);
        return View(entry);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrackerEntry entry)
    {
        if (id != entry.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _db.Update(entry);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Tracker entry updated.";
            return RedirectToAction(nameof(Index));
        }

        await PopulateLookups(entry.WeekId, entry.DeveloperId);
        return View(entry);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var entry = await _db.TrackerEntries
            .Include(t => t.Week)
            .Include(t => t.Developer)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (entry == null)
        {
            return NotFound();
        }

        return View(entry);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entry = await _db.TrackerEntries.FindAsync(id);
        if (entry != null)
        {
            _db.TrackerEntries.Remove(entry);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Tracker entry deleted.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateLookups(int? weekId = null, int? developerId = null)
    {
        ViewBag.WeekId = new SelectList(await _db.Weeks.OrderBy(w => w.WeekNumber).ToListAsync(), "Id", "Label", weekId);
        ViewBag.DeveloperId = new SelectList(await _db.Developers.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", developerId);
    }
}
