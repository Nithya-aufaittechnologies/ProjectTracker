using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MpcFabricsTracker.Models;

public class WeeklyCapacity
{
    public int Id { get; set; }

    [Required]
    public int WeekId { get; set; }
    public Week? Week { get; set; }

    [Required]
    public int DeveloperId { get; set; }
    public Developer? Developer { get; set; }

    [Display(Name = "Planned Hrs")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal PlannedHours { get; set; }

    [Display(Name = "Planned Leave Hrs")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal PlannedLeaveHours { get; set; }

    [Display(Name = "Assigned Hrs")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal AssignedHours { get; set; }

    [Display(Name = "Total Task Assigned")]
    public int TotalTaskAssigned { get; set; }

    [NotMapped]
    [Display(Name = "Actual Capacity")]
    public decimal ActualCapacity => PlannedHours - PlannedLeaveHours;
}
