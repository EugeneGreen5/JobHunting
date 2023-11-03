using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace JobHunting.Models.Entity;

public class Person : BaseEntity
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
    /// Телефон пользователя \\ Example +7(911)345-67-89
    /// </summary>
    public string Phone { get; set; }
    /// <summary>
    /// Город, в котором пользователь ищет работу
    /// </summary>
    public City? City { get; set; }
    public string Password { get; set; }

    [JsonIgnore]
    public string Role { get; set; } = "user";

    [JsonIgnore]
    public DecodeMethod DecodeMethod { get; set; }
    /// <summary>
    /// Резюме пользователя
    /// </summary>
    public virtual List<Resume>? Resumes { get; set; }
    public DateTime StartSession { get; set; } = DateTime.Now;
    public Person()
    {
        Id = Guid.NewGuid();
    }

}