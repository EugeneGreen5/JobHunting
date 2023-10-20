using JobHunting.Controllers;

namespace JobHunting.Models.Entity;

public class Resume : BaseEntity
{
    /// <summary>
    /// Поле "Обо мне"
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Желаемая зарплата
    /// </summary>
    public int? Salary { get; set; }

    public bool IsFullEmployment { get; set; } = false;
    public bool IsPartTimeEmployment { get; set; } = false;
    public bool IsVolunteering { get; set; } = false;
    public bool IsTrainee { get; set; } = false;

    public Guid PersonId { get; set; }
    public Resume()
    {
        Id = Guid.NewGuid();
    }
}