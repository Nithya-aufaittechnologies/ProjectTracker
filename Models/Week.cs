using System.ComponentModel.DataAnnotations;

namespace MpcFabricsTracker.Models;

public class Week
{
    public int Id { get; set; }

    [Display(Name = "Week No")]
    public int WeekNumber { get; set; }

    [Display(Name = "Week Start (Mon)")]
    [DataType(DataType.Date)]
    public DateTime WeekStart { get; set; }

    [StringLength(50)]
    public string Label { get; set; } = string.Empty;

    public ICollection<TrackerEntry> TrackerEntries { get; set; } = new List<TrackerEntry>();
    public ICollection<WeeklyCapacity> WeeklyCapacities { get; set; } = new List<WeeklyCapacity>();
}
