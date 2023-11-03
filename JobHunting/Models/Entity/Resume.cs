using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHunting.Models.Entity;

[Table("resumes")]
[Index("Salary")]
public class Resume : BaseEntity
{
    /// <summary>
    /// Поле "Обо мне"
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    /// <summary>
    /// Желаемая зарплата
    /// </summary>
    [Required]
    public int Salary { get; set; }
    [Column("full_employment")]
    public bool IsFullEmployment { get; set; } = false;
    [Column("part_time_employment")]
    public bool IsPartTimeEmployment { get; set; } = false;
    [Column("volunteering")]
    public bool IsVolunteering { get; set; } = false;
    [Column("trainee")]
    public bool IsTrainee { get; set; } = false;
    [NotMapped]
    public bool IsActive { get; set; } = true;
    public Guid PersonId { get; set; }
    public Resume()
    {
        Id = Guid.NewGuid();
    }
}