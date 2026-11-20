namespace MpcFabricsTracker.Models.ViewModels;

public class SummaryRowViewModel
{
    public int WeekNumber { get; set; }
    public string WeekLabel { get; set; } = string.Empty;
    public DateTime WeekStart { get; set; }
    public int DeveloperId { get; set; }
    public string DeveloperName { get; set; } = string.Empty;
    public decimal PlannedHours { get; set; }
    public decimal PlannedLeaveHours { get; set; }
    public decimal ActualCapacity { get; set; }
    public decimal AssignedHours { get; set; }
    public decimal SpentHours { get; set; }
    public int TotalTaskAssigned { get; set; }
    public int CompletedTask { get; set; }
    public decimal ProductivityPercent { get; set; }
    public decimal UtilizationPercent { get; set; }
}

public class SummaryPageViewModel
{
    public List<string> DeveloperNames { get; set; } = new();
    public List<int> WeekNumbers { get; set; } = new();
    public List<SummaryRowViewModel> Rows { get; set; } = new();
    public int? SelectedWeek { get; set; }
    public int? SelectedDeveloperId { get; set; }
}
