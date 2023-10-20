using System.ComponentModel;

namespace JobHunting.Models;

/// <summary>
/// Enum для городов
/// </summary>
public enum City
{
    [Description("Москва")]
    Moscow,

    [Description("Саратов")]
    Saratov,

    [Description("Самара")]
    Samara,

    [Description("Санкт-Петербург")]
    SaintPetersburg,

    [Description("Калининград")]
    Kaliningrad

}