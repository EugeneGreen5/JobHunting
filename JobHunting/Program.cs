using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

internal class Program
{
    /// <summary>
    /// ������ ��� �������� �������������
    /// </summary>
    public static List<Person> persons = new List<Person>(); 
    /// <summary>
    /// ������ ��� �������� ������
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
                Description = "����������� API"
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
        #region ����� ��� ������
        resumesMaps.MapGet("/", GetAllResumesAsync);
        resumesMaps.MapGet("/{id}", GetResumeByIdAsync);
        resumesMaps.MapPost("/", AddResumeAsync);
        resumesMaps.MapDelete("/{id}", DeleteResumeByIdAsync);
        #endregion

        var personsMaps = app.MapGroup("/person");
        #region ����� ��� �������������

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

    #region ���������� ������� ��� ����� ��� ������

    /// <summary>
    /// ����� ��� ��������� ���� ������
    /// </summary>
    /// <returns>������ ������</returns>
    public static async Task<IResult> GetAllResumesAsync() =>
        TypedResults.Ok(resumes);

    /// <summary>
    /// ����� ��� ��������� ������ �� id
    /// </summary>
    /// <param name="id">id ������</param>
    /// <returns>404 - ������ �� �������</returns>
    /// <returns>200 - ���������� ������</returns>
    public static async Task<IResult> GetResumeByIdAsync(Guid id)
    {
        var currentResume = resumes.FirstOrDefault(c => c.Id == id);
        if (currentResume is null) return TypedResults.NotFound();

        return TypedResults.Ok(currentResume);
    }

    /// <summary>
    /// ����� ��� ���������� ������
    /// </summary>
    /// <param name="resume">������, ������� ����� ��������</param>
    /// <returns>422 - ���� ������ ������ ������</returns>
    /// <returns>201 - ���� ������ ���������</returns>
    public static async Task<IResult> AddResumeAsync(Resume resume)
    {
        if (resume is null) return TypedResults.UnprocessableEntity();

        resumes.Add(resume);
        return TypedResults.Created($"/resume/{resume.Id}", resume);
    }
    /// <summary>
    /// ����� ��� �������� ������
    /// </summary>
    /// <param name="id">id ������</param>
    /// <returns>404 - ���� ������ �� �������</returns>
    /// <returns>204 - ���� ������ �������</returns>
    public static async Task<IResult> DeleteResumeByIdAsync(Guid id)
    {
        var currentResume = resumes.FirstOrDefault(c => c.Id == id);
        if (currentResume is null) return TypedResults.NotFound();

        resumes.Remove(currentResume);
        return TypedResults.NoContent();
    }


    #endregion


    #region ���������� ������� ��� ����� ��� ������������

    /// <summary>
    /// ����� ��� ��������� ���� �������������
    /// </summary>
    /// <returns>200 - ���������� ������ �������������</returns>
    public static async Task<IResult> GetAllPersonsAsync() =>
        TypedResults.Ok(persons);

    /// <summary>
    /// ����� ��� ��������� ������������ �� id
    /// </summary>
    /// <param name="id">id ������������</param>
    /// <returns>404 - ���� ������������ �� ������</returns>
    /// <returns>200 - ���������� ������������</returns>
    public static async Task<IResult> GetPersonByIdAsync(Guid id)
    {
        var currentPerson = persons.FirstOrDefault(a => a.Id == id);
        if (currentPerson is null) return TypedResults.NotFound();

        return TypedResults.Ok(currentPerson);
    }

    /// <summary>
    /// ����� ��� ���������� ������������
    /// </summary>
    /// <param name="newPerson">������ ������������, �������� ����� ��������</param>
    /// <returns>422 - ���� ������ ����������� ������</returns>
    /// <returns>201 - ���� ������������ ������� ��������</returns>
    public static async Task<IResult> AddPersonAsync(Person newPerson)
    {
        if (newPerson is null) return TypedResults.UnprocessableEntity();
        newPerson.Resume = new Resume();

        persons.Add(newPerson);
        return TypedResults.Created($"/person/{newPerson.Id}", newPerson);
    }
    
    /// <summary>
    /// ����� ��� �������� ������������ + ������� ��� ������ 
    /// </summary>
    /// <param name="id">id ������������</param>
    /// <returns>404 - ���� ������������ �� ������</returns>
    /// <returns>204 - ���� ������������ �����</returns>
    public static async Task<IResult> DeletePersonByIdAsync(Guid id)
    {
        var currentPerson = persons.FirstOrDefault(p => p.Id == id);
        if (currentPerson is null) return TypedResults.NotFound();

        persons.Remove(currentPerson);
        await DeleteResumeByIdAsync(currentPerson.Resume.Id);
        return TypedResults.NoContent();
    }
    
    /// <summary>
    /// ����� ��� ���������� ������ � ������������ + ������� ������ ������ + ��������� ����� ������
    /// </summary>
    /// <param name="id">id ������������</param>
    /// <param name="newResume">������ ������</param>
    /// <returns>404 - ���� ������������ �� ������</returns>
    /// <returns>200 - ���� ������������ ������� �������</returns>
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
    /// ����� ��� ��������� ������ �� ��������� (��������)
    /// </summary>
    /// <param name="employment">��� ���������</param>
    /// <returns>404 - ���� ������������ �� ������</returns>
    /// <returns>200 - ���������� ������ �������������, � ������� ���� ������� ��������</returns>
    public static async Task<IResult> GetPersonByEmployment(Employment employment)
    {
        var filteredPersons = persons.Where(c => c.Resume.Employment.Any(p => p == employment)).ToList();
        if (filteredPersons is null) return TypedResults.NotFound();

        return TypedResults.Ok(filteredPersons);
    }
    #endregion

    #region ��� ������
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
            Name = "������� ������ ���������",
            Email = "fyodor70@yahoo.com",
            Phone = "+7(448)138-76-12",
            City = City.Moscow,
            Resume = thirdResume
        };
        Person secondPerson = new Person()
        {
            Name = "�������� ��������� ��������",
            Email = "zhanna_efimova@yandex.ru",
            Phone = "+7(918)459-00-38",
            City = City.Samara,
            Resume = secondResume
        };
        Person thirdPerson = new Person()
        {
            Name = "�������� ������ ���������",
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

    #region ��������
    /// <summary>
    /// �������� ������������
    /// </summary>
    public class Person
    {
        /// <summary>
        /// ���������� �������������
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// ��� ������������
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Email ������������
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ������� ������������
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// �����, � ������� ������������ ���� ������
        /// </summary>
        public City? City { get; set; } 
        /// <summary>
        /// ������ ������������
        /// </summary>
        public Resume? Resume { get; set; }
        public Person()
        {
            Id = Guid.NewGuid();

        }

    }

    /// <summary>
    /// �������� ������
    /// </summary>
    public class Resume
    {
        /// <summary>
        /// ���������� �������������
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// ���� "��� ���"
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// �������� ��������
        /// </summary>
        public int? Salary { get; set; }
        /// <summary>
        /// ������ ���������
        /// </summary>
        public List<Employment> Employment { get; set; }
        public Resume()
        {
            Id = Guid.NewGuid();
        }
    }

    /// <summary>
    /// Enum ��� �������
    /// </summary>
    public enum City
    {
        [Description("������")]
        Moscow,

        [Description("�������")]
        Saratov,

        [Description("������")]
        Samara,

        [Description("�����-���������")]
        SaintPetersburg,

        [Description("�����������")]
        Kaliningrad

    }

    /// <summary>
    /// Enum ��� ���������
    /// </summary>
    public enum Employment
    {
        [Description("������ ���������")]
        FullEmployment,

        [Description("��������� ���������")]
        PartTimeEmployment,

        [Description("�����������")]
        Volunteering,

        [Description("����������")]
        Trainee
    }
    #endregion
}