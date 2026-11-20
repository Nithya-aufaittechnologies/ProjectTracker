using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MpcFabricsTracker.Data;
using MpcFabricsTracker.Services;

namespace MpcFabricsTracker.Controllers;

public class SummaryController : Controller
{
    private readonly SummaryCalculatorService _summaryService;
    private readonly AppDbContext _db;

    public SummaryController(SummaryCalculatorService summaryService, AppDbContext db)
    {
        _summaryService = summaryService;
        _db = db;
    }

    public async Task<IActionResult> Index(int? weekNumber, int? developerId)
    {
        var model = await _summaryService.BuildAsync(weekNumber, developerId);
        ViewBag.WeekNumber = new SelectList(
            await _db.Weeks.OrderBy(w => w.WeekNumber).ToListAsync(),
            "WeekNumber",
            "Label",
            weekNumber);
        ViewBag.DeveloperId = new SelectList(
            await _db.Developers.OrderBy(d => d.Name).ToListAsync(),
            "Id",
            "Name",
            developerId);
        return View(model);
    }
}
