using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

internal class Program
{
    /// <summary>
    /// Список для хранения пользователей
    /// </summary>
    public static List<Person> persons = new List<Person>(); 
    /// <summary>
    /// Список для хранения резюме
    /// </summary>
    public static List<Resume> resumes = new List<Resume>();

    private static void Main(string[] args)
    {
        MockData();
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "JobHunting API",
                Description = "Минимальное API"
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        var resumesMaps = app.MapGroup("/resume");
        #region Ручки для резюме
        resumesMaps.MapGet("/", GetAllResumesAsync);
        resumesMaps.MapGet("/{id}", GetResumeByIdAsync);
        resumesMaps.MapPost("/", AddResumeAsync);
        resumesMaps.MapDelete("/{id}", DeleteResumeByIdAsync);
        #endregion

        var personsMaps = app.MapGroup("/person");
        #region Ручки для пользователей

        personsMaps.MapGet("/", GetAllPersonsAsync);
        personsMaps.MapGet("/{id}", GetPersonByIdAsync);
        personsMaps.MapPost("", AddPersonAsync);
        personsMaps.MapDelete("/{id}", DeletePersonByIdAsync);
        personsMaps.MapPatch("/{id}", UpdateResumeByIdAsync);
        personsMaps.MapGet("/employment/{employment}", GetPersonByEmployment);
        #endregion

        //app.MapGet("/", () => "Hello World!");

        app.Run();
    }

    #region Реализации методов для ручек под резюме

    /// <summary>
    /// Метод для получения всех резюме
    /// </summary>
    /// <returns>Список резюме</returns>
    public static async Task<IResult> GetAllResumesAsync() =>
        TypedResults.Ok(resumes);

    /// <summary>
    /// Метод для получения резюме по id
    /// </summary>
    /// <param name="id">id резюме</param>
    /// <returns>404 - резюме не найдено</returns>
    /// <returns>200 - возвращает резюме</returns>
    public static async Task<IResult> GetResumeByIdAsync(Guid id)
    {
        var currentResume = resumes.FirstOrDefault(c => c.Id == id);
        if (currentResume is null) return TypedResults.NotFound();

        return TypedResults.Ok(currentResume);
    }

    /// <summary>
    /// Метод для добавления резюме
    /// </summary>
    /// <param name="resume">Резюме, которое нужно добавить</param>
    /// <returns>422 - если пришло пустое резюме</returns>
    /// <returns>201 - если резюме добавлено</returns>
    public static async Task<IResult> AddResumeAsync(Resume resume)
    {
        if (resume is null) return TypedResults.UnprocessableEntity();

        resumes.Add(resume);
        return TypedResults.Created($"/resume/{resume.Id}", resume);
    }
    /// <summary>
    /// Метод для удаления резюме
    /// </summary>
    /// <param name="id">id резюме</param>
    /// <returns>404 - если резюме не найдено</returns>
    /// <returns>204 - если резюме удалено</returns>
    public static async Task<IResult> DeleteResumeByIdAsync(Guid id)
    {
        var currentResume = resumes.FirstOrDefault(c => c.Id == id);
        if (currentResume is null) return TypedResults.NotFound();

        resumes.Remove(currentResume);
        return TypedResults.NoContent();
    }


    #endregion


    #region Реализация методов для ручек под пользователя

    /// <summary>
    /// Метод для получения всех пользователей
    /// </summary>
    /// <returns>200 - возвращает список пользователей</returns>
    public static async Task<IResult> GetAllPersonsAsync() =>
        TypedResults.Ok(persons);

    /// <summary>
    /// Метод для получения пользователя по id
    /// </summary>
    /// <param name="id">id пользователя</param>
    /// <returns>404 - если пользователь не найден</returns>
    /// <returns>200 - возвращает пользователя</returns>
    public static async Task<IResult> GetPersonByIdAsync(Guid id)
    {
        var currentPerson = persons.FirstOrDefault(a => a.Id == id);
        if (currentPerson is null) return TypedResults.NotFound();

        return TypedResults.Ok(currentPerson);
    }

    /// <summary>
    /// Метод для добавления пользователя
    /// </summary>
    /// <param name="newPerson">Объект пользователя, которого нужно добавить</param>
    /// <returns>422 - если пришёл бракованный объект</returns>
    /// <returns>201 - если пользователь успешно добавлен</returns>
    public static async Task<IResult> AddPersonAsync(Person newPerson)
    {
        if (newPerson is null) return TypedResults.UnprocessableEntity();
        newPerson.Resume = new Resume();

        persons.Add(newPerson);
        return TypedResults.Created($"/person/{newPerson.Id}", newPerson);
    }
    
    /// <summary>
    /// Метод для удаления пользователя + удаляет его резюме 
    /// </summary>
    /// <param name="id">id пользователя</param>
    /// <returns>404 - если пользователь не найден</returns>
    /// <returns>204 - если пользователь удалён</returns>
    public static async Task<IResult> DeletePersonByIdAsync(Guid id)
    {
        var currentPerson = persons.FirstOrDefault(p => p.Id == id);
        if (currentPerson is null) return TypedResults.NotFound();

        persons.Remove(currentPerson);
        await DeleteResumeByIdAsync(currentPerson.Resume.Id);
        return TypedResults.NoContent();
    }
    
    /// <summary>
    /// Метод для обновления резюме у пользователя + удаляет старое резюме + добавляет новое резюме
    /// </summary>
    /// <param name="id">id пользователя</param>
    /// <param name="newResume">объект резюме</param>
    /// <returns>404 - если пользователь не найден</returns>
    /// <returns>200 - если пользователь успешно обновлён</returns>
    public static async Task<IResult> UpdateResumeByIdAsync(Guid id, Resume newResume)
    {
        var currentPerson = persons.FirstOrDefault(c => c.Id == id);
        if (currentPerson is null) return TypedResults.NotFound();

        await DeleteResumeByIdAsync(currentPerson.Resume.Id);
        currentPerson.Resume = newResume;
        await AddResumeAsync(newResume);
        return TypedResults.Ok();
    }

    /// <summary>
    /// Метод для получения списка по занятости (параметр)
    /// </summary>
    /// <param name="employment">тип занятости</param>
    /// <returns>404 - если пользователь не найден</returns>
    /// <returns>200 - возвращает список пользователей, у которых есть входной параметр</returns>
    public static async Task<IResult> GetPersonByEmployment(Employment employment)
    {
        var filteredPersons = persons.Where(c => c.Resume.Employment.Any(p => p == employment)).ToList();
        if (filteredPersons is null) return TypedResults.NotFound();

        return TypedResults.Ok(filteredPersons);
    }
    #endregion

    #region Мок данных
    private static void MockData()
    {
        Resume firstResume = new Resume()
        {
            Description = "123",
            Salary = 230000,
            Employment = new List<Employment> {
                Employment.FullEmployment ,
                Employment.Trainee,
                Employment.PartTimeEmployment
            }

        };
        Resume secondResume = new Resume()
        {
            Description = "456",
            Salary = 25000,
            Employment = new List<Employment> {
                Employment.Trainee
            }

        };
        Resume thirdResume = new Resume()
        {
            Description = "789",
            Salary = 90020210,
            Employment = new List<Employment> {
                Employment.FullEmployment ,
                Employment.Volunteering
            }

        };

        Person firstPerson = new Person()
        {
            Name = "Корнеев Руслан Романович",
            Email = "fyodor70@yahoo.com",
            Phone = "+7(448)138-76-12",
            City = City.Moscow,
            Resume = thirdResume
        };
        Person secondPerson = new Person()
        {
            Name = "Баранова Екатерина Ивановна",
            Email = "zhanna_efimova@yandex.ru",
            Phone = "+7(918)459-00-38",
            City = City.Samara,
            Resume = secondResume
        };
        Person thirdPerson = new Person()
        {
            Name = "Михайлов Даниил Гордеевич",
            Email = "lyudmila_yudina@mail.ru",
            Phone = "+7(582)948-84-85",
            City = City.Saratov,
            Resume = firstResume
        };

        resumes.Add(firstResume);
        resumes.Add(secondResume);
        resumes.Add(thirdResume);

        persons.Add(firstPerson);
        persons.Add(secondPerson);
        persons.Add(thirdPerson);

    }
    #endregion

    #region Сущности
    /// <summary>
    /// Сущность Пользователя
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Id { get; init; }
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
        /// <summary>
        /// Резюме пользователя
        /// </summary>
        public Resume? Resume { get; set; }
        public Person()
        {
            Id = Guid.NewGuid();

        }

    }

    /// <summary>
    /// Сущность Резюме
    /// </summary>
    public class Resume
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// Поле "Обо мне"
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Желаемая зарплата
        /// </summary>
        public int? Salary { get; set; }
        /// <summary>
        /// Список занятости
        /// </summary>
        public List<Employment> Employment { get; set; }
        public Resume()
        {
            Id = Guid.NewGuid();
        }
    }

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
    #endregion
}