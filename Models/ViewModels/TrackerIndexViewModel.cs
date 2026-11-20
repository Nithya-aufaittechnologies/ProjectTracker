using Microsoft.AspNetCore.Mvc.Rendering;

namespace MpcFabricsTracker.Models.ViewModels;

public class TrackerIndexViewModel
{
    public List<TrackerEntry> Entries { get; set; } = new();
    public int? WeekNumber { get; set; }
    public int? DeveloperId { get; set; }
    public SelectList? Weeks { get; set; }
    public SelectList? Developers { get; set; }
}
