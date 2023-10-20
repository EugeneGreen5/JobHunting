using System.ComponentModel;

namespace JobHunting.Models;

/// <summary>
/// Enum для занятости
/// </summary>
public enum Employment
{
    [Description("Полная занятость")]
    FullEmployment,

    [Description("Частичная занятость")]
    PartTimeEmployment,

    [Description("Волонтёрство")]
    Volunteering,

    [Description("Стажировка")]
    Trainee
}
