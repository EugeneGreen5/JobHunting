using JobHunting.Controllers;

namespace JobHunting.Models.DTO;

public class PersonDTO
{
    /// <summary>
    /// ФИО пользователя
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Телефон пользователя
    /// </summary>
    public string Phone { get; set; }
    /// <summary>
    /// Город, в котором пользователь ищет работу
    /// </summary>
    public City? City { get; set; }
}