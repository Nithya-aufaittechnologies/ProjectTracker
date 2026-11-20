using Microsoft.EntityFrameworkCore;
using MpcFabricsTracker.Data;
using MpcFabricsTracker.Models.ViewModels;

namespace MpcFabricsTracker.Services;

public class SummaryCalculatorService
{
    private readonly AppDbContext _db;

    public SummaryCalculatorService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SummaryPageViewModel> BuildAsync(int? weekNumber = null, int? developerId = null)
    {
        var developers = await _db.Developers
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();

        var weeksQuery = _db.Weeks.AsQueryable();
        if (weekNumber.HasValue)
        {
            weeksQuery = weeksQuery.Where(w => w.WeekNumber == weekNumber.Value);
        }

        var weeks = await weeksQuery.OrderBy(w => w.WeekNumber).ToListAsync();

        var capacities = await _db.WeeklyCapacities
            .Include(c => c.Week)
            .Include(c => c.Developer)
            .ToListAsync();

        var entries = await _db.TrackerEntries
            .Include(t => t.Week)
            .Include(t => t.Developer)
            .ToListAsync();

        var rows = new List<SummaryRowViewModel>();

        foreach (var week in weeks)
        {
            var weekDevelopers = developers.AsEnumerable();
            if (developerId.HasValue)
            {
                weekDevelopers = weekDevelopers.Where(d => d.Id == developerId.Value);
            }

            foreach (var developer in weekDevelopers)
            {
                var capacity = capacities.FirstOrDefault(c =>
                    c.WeekId == week.Id && c.DeveloperId == developer.Id);

                var weekEntries = entries
                    .Where(e => e.WeekId == week.Id && e.DeveloperId == developer.Id)
                    .ToList();

                // Same logic as Excel: Spent Hrs where % Completed > 0
                var spentHours = weekEntries
                    .Where(e => e.PercentCompleted > 0)
                    .Sum(e => e.SpendHours);

                // Same logic as Excel: Completed Task where % Completed >= 1
                var completedTask = weekEntries.Count(e => e.PercentCompleted >= 1m);

                var planned = capacity?.PlannedHours ?? 0;
                var leave = capacity?.PlannedLeaveHours ?? 0;
                var actualCapacity = planned - leave;
                var assignedHours = capacity?.AssignedHours ?? 0;
                var totalAssigned = capacity?.TotalTaskAssigned ?? 0;

                var productivity = totalAssigned == 0
                    ? 0
                    : Math.Round((decimal)completedTask / totalAssigned * 100m, 1);

                var utilization = actualCapacity <= 0
                    ? 0
                    : Math.Round(spentHours / actualCapacity * 100m, 1);

                rows.Add(new SummaryRowViewModel
                {
                    WeekNumber = week.WeekNumber,
                    WeekLabel = string.IsNullOrWhiteSpace(week.Label) ? $"Week {week.WeekNumber}" : week.Label,
                    WeekStart = week.WeekStart,
                    DeveloperId = developer.Id,
                    DeveloperName = developer.Name,
                    PlannedHours = planned,
                    PlannedLeaveHours = leave,
                    ActualCapacity = actualCapacity,
                    AssignedHours = assignedHours,
                    SpentHours = spentHours,
                    TotalTaskAssigned = totalAssigned,
                    CompletedTask = completedTask,
                    ProductivityPercent = productivity,
                    UtilizationPercent = utilization
                });
            }
        }

        return new SummaryPageViewModel
        {
            DeveloperNames = developers.Select(d => d.Name).ToList(),
            WeekNumbers = await _db.Weeks.OrderBy(w => w.WeekNumber).Select(w => w.WeekNumber).ToListAsync(),
            Rows = rows,
            SelectedWeek = weekNumber,
            SelectedDeveloperId = developerId
        };
    }
}
