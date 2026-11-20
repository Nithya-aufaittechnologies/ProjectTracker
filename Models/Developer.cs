using System.ComponentModel.DataAnnotations;

namespace MpcFabricsTracker.Models;

public class Developer
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<TrackerEntry> TrackerEntries { get; set; } = new List<TrackerEntry>();
    public ICollection<WeeklyCapacity> WeeklyCapacities { get; set; } = new List<WeeklyCapacity>();
}
