using Microsoft.EntityFrameworkCore;
using MpcFabricsTracker.Models;

namespace MpcFabricsTracker.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (await db.Developers.AnyAsync())
        {
            return;
        }

        var developers = new[]
        {
            new Developer { Name = "Shadha" },
            new Developer { Name = "Noof" },
            new Developer { Name = "Navya EM" },
            new Developer { Name = "Navya K K" },
            new Developer { Name = "Nishad" }
        };
        db.Developers.AddRange(developers);

        var weeks = new[]
        {
            new Week { WeekNumber = 1, WeekStart = new DateTime(2026, 10, 26), Label = "Week 1" },
            new Week { WeekNumber = 2, WeekStart = new DateTime(2026, 11, 2), Label = "Week 2" },
            new Week { WeekNumber = 3, WeekStart = new DateTime(2026, 11, 9), Label = "Week 3" },
            new Week { WeekNumber = 4, WeekStart = new DateTime(2026, 11, 16), Label = "Week 4" },
            new Week { WeekNumber = 5, WeekStart = new DateTime(2026, 11, 23), Label = "Week 5" },
            new Week { WeekNumber = 6, WeekStart = new DateTime(2026, 11, 30), Label = "Week 6" }
        };
        db.Weeks.AddRange(weeks);
        await db.SaveChangesAsync();

        // Seed weekly capacity from Summary sheet (manual blue cells)
        var capacitySeeds = new (string Dev, int Week, decimal Planned, decimal Leave, decimal Assigned, int Tasks)[]
        {
            ("Shadha", 1, 32, 16, 6, 3),
            ("Shadha", 2, 40, 0, 35, 8),
            ("Shadha", 3, 32, 0, 22, 6),
            ("Shadha", 4, 40, 0, 40, 5),
            ("Shadha", 5, 40, 0, 40, 5),
            ("Shadha", 6, 40, 0, 40, 5),
            ("Noof", 1, 32, 8, 24, 5),
            ("Noof", 2, 40, 0, 40, 8),
            ("Noof", 3, 32, 8, 24, 6),
            ("Noof", 4, 40, 0, 32, 5),
            ("Noof", 5, 40, 0, 32, 5),
            ("Noof", 6, 40, 0, 32, 5),
            ("Navya EM", 1, 32, 8, 0, 5),
            ("Navya EM", 2, 40, 0, 16, 5),
            ("Navya EM", 3, 32, 4, 8, 5),
            ("Navya EM", 4, 40, 0, 16, 5),
            ("Navya EM", 5, 40, 0, 16, 5),
            ("Navya EM", 6, 40, 0, 16, 5),
            ("Navya K K", 1, 32, 16, 16, 5),
            ("Navya K K", 2, 40, 0, 40, 8),
            ("Navya K K", 3, 32, 0, 32, 6),
            ("Navya K K", 4, 40, 0, 40, 5),
            ("Navya K K", 5, 40, 0, 40, 5),
            ("Navya K K", 6, 40, 0, 40, 5),
            ("Nishad", 1, 32, 0, 28, 5),
            ("Nishad", 2, 40, 0, 40, 8),
            ("Nishad", 3, 32, 0, 32, 6),
            ("Nishad", 4, 40, 0, 40, 5),
            ("Nishad", 5, 40, 0, 40, 5),
            ("Nishad", 6, 40, 0, 40, 5)
        };

        foreach (var seed in capacitySeeds)
        {
            var developer = developers.First(d => d.Name == seed.Dev);
            var week = weeks.First(w => w.WeekNumber == seed.Week);
            db.WeeklyCapacities.Add(new WeeklyCapacity
            {
                DeveloperId = developer.Id,
                WeekId = week.Id,
                PlannedHours = seed.Planned,
                PlannedLeaveHours = seed.Leave,
                AssignedHours = seed.Assigned,
                TotalTaskAssigned = seed.Tasks
            });
        }

        // Representative Tracker rows from the Excel Tracker sheet
        var trackerSeeds = new (int Week, string Date, string Dev, string Task, string Activity, decimal Planned, decimal Spend, decimal Pct)[]
        {
            (1, "2026-10-27", "Noof", "Private Sales by Customer", "Semantic Model", 4, 4, 1),
            (1, "2026-10-27", "Noof", "Private Sales by Customer", "DAX", 4, 4, 1),
            (1, "2026-10-27", "Nishad", "Private Sales by Product Overview", "Semantic model", 4, 4, 1),
            (1, "2026-10-27", "Nishad", "Private Sales by Product Overview", "DAX", 4, 4, 1),
            (1, "2026-10-27", "Navya K K", "Executive Summary", "Semantic Model, DAX", 8, 8, 1),
            (1, "2026-10-28", "Noof", "Private Sales by Customer", "Report Development", 8, 8, 1),
            (1, "2026-10-28", "Nishad", "Private Sales by Product Overview", "Report Development", 8, 8, 1),
            (1, "2026-10-28", "Navya K K", "Executive Summary", "Report Development", 8, 8, 1),
            (1, "2026-10-30", "Shadha", "Aufait Tenant Data Warehousing - Test", "Same tenant warehouse migration", 6, 6, 1),
            (2, "2026-11-02", "Noof", "Private Sales by Product", "Report Development", 8, 8, 1),
            (2, "2026-11-02", "Nishad", "Retail Sales by Product", "DAX, Calculation", 6, 6, 1),
            (2, "2026-11-02", "Navya K K", "Private Sales by Consumer Division", "DAX & Calculations", 8, 8, 1),
            (2, "2026-11-02", "Shadha", "Aufait Tenant Data Warehousing - Test", "Semantic model and Power BI migration", 8, 8, 1),
            (2, "2026-11-02", "Navya EM", "Aufait Tenant Data Warehousing - Test", "fact_inventory creation", 8, 8, 0.5m),
            (2, "2026-11-03", "Noof", "Private Sales Team Performance - Pharma Division", "Dax, Calculation", 8, 8, 1),
            (2, "2026-11-03", "Nishad", "Retail Sales by Product", "Report Development", 4, 4, 1),
            (2, "2026-11-03", "Shadha", "Aufait Tenant Data Warehousing - Test", "Cross tenant warehouse migration", 5, 5, 1),
            (2, "2026-11-03", "Navya EM", "Aufait Tenant Data Warehousing - Test", "fact_inventory creation", 8, 8, 0.5m),
            (3, "2026-11-10", "Nishad", "Expiry / Near Expiry Overview", "Report Development, DAX", 5, 5, 1),
            (3, "2026-11-10", "Nishad", "Inventory Overview (Retail)", "DAX calculation", 3, 3, 1),
            (3, "2026-11-10", "Navya K K", "Inventory Executive Dashboard", "Report Development", 8, 8, 1),
            (3, "2026-11-10", "Noof", "Retail Executive Dashboard", "Report Development", 3, 3, 1),
            (3, "2026-11-10", "Shadha", "Aufait Tenant Data Warehousing - Test", "sales pipeline optimization", 5, 5, 1),
            (3, "2026-11-11", "Shadha", "Fabric-Workspace Setup and warehouse", "warehouse creation, schema and table", 4, 4, 1),
            (3, "2026-11-12", "Shadha", "Fabric-Workspace and warehouse setup", "master data pipeline", 2, 2, 0.2m),
            (3, "2026-11-13", "Navya EM", "Target Pipeline", "Excel migration, branch target pipeline", 8, 8, 1),
            (3, "2026-11-13", "Shadha", "Sales Pipeline- incremental refresh", "master data pipeline", 8, 8, 0.5m),
            (3, "2026-11-13", "Noof", "Retail Health Scorecard", "Dax and calculations", 4, 4, 1),
            (4, "2026-11-16", "Navya EM", "Target Pipeline", "salesman target pipeline development", 8, 8, 1),
            (4, "2026-11-17", "Navya EM", "Target Pipeline", "Retail target pipeline development", 2, 3, 1),
            (4, "2026-11-17", "Shadha", "Sales Pipeline- incremental refresh", "master data pipeline", 4, 4, 1),
            (4, "2026-11-17", "Shadha", "Sales Pipeline- incremental refresh", "bronze transaction-2026", 4, 4, 1),
            (4, "2026-11-18", "Shadha", "Sales Pipeline- incremental refresh", "silver transaction-2026 and fact_Sales", 8, 8, 0.6m),
            (4, "2026-11-19", "Shadha", "Sales Pipeline- incremental refresh", "Daily Sales Pipeline Troubleshooting", 2, 2, 1),
            (4, "2026-11-19", "Navya EM", "Inventory Pipeline", "stock historic", 8, 0, 0),
            (4, "2026-11-19", "Shadha", "Historical data migration", "2025-fact_sales", 6, 0, 0.5m),
            (4, "2026-11-20", "Navya EM", "Inventory Pipeline", "current stock and abc classification", 8, 0, 0),
            (4, "2026-11-20", "Shadha", "Historical data migration", "2024-fact_Sales", 8, 0, 0),
            (5, "2026-11-23", "Navya EM", "Inventory Pipeline", "fact inventory", 8, 0, 0),
            (5, "2026-11-23", "Shadha", "Historical data migration", "2023-fact_sales", 8, 0, 0)
        };

        foreach (var seed in trackerSeeds)
        {
            var developer = developers.First(d => d.Name == seed.Dev);
            var week = weeks.First(w => w.WeekNumber == seed.Week);
            db.TrackerEntries.Add(new TrackerEntry
            {
                WeekId = week.Id,
                WorkDate = DateTime.Parse(seed.Date),
                DeveloperId = developer.Id,
                DashboardTask = seed.Task,
                DayWiseActivity = seed.Activity,
                PlannedHours = seed.Planned,
                SpendHours = seed.Spend,
                PercentCompleted = seed.Pct
            });
        }

        await db.SaveChangesAsync();
    }
}
