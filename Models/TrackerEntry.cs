using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MpcFabricsTracker.Models;

public class TrackerEntry
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Week")]
    public int WeekId { get; set; }
    public Week? Week { get; set; }

    [Required]
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    public DateTime WorkDate { get; set; }

    [Required]
    [Display(Name = "Developer")]
    public int DeveloperId { get; set; }
    public Developer? Developer { get; set; }

    [Required, StringLength(200)]
    [Display(Name = "Dashboard / Task")]
    public string DashboardTask { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Day-wise Activity")]
    public string? DayWiseActivity { get; set; }

    [Display(Name = "Planned Hours")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal PlannedHours { get; set; }

    [Display(Name = "Spend Hours")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal SpendHours { get; set; }

    [Display(Name = "% Completed")]
    [Range(0, 1)]
    [Column(TypeName = "decimal(5,2)")]
    public decimal PercentCompleted { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    [NotMapped]
    [Display(Name = "Status")]
    public string Status =>
        PercentCompleted >= 1m ? "Completed"
        : PercentCompleted > 0m ? "In Progress"
        : "Not Started";

    [NotMapped]
    public string DayName => WorkDate.ToString("ddd");
}
