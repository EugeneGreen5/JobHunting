using System.ComponentModel;

internal class Program
{
    public static List<Person> Persons { get; set; }
    public static List<Resume> Resumes { get; set; }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");


        MockData();


        app.Run();
    }

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

        Resumes.Add(firstResume);
        Resumes.Add(secondResume);
        Resumes.Add(thirdResume);

        Persons.Add(firstPerson);
        Persons.Add(secondPerson);
        Persons.Add(thirdPerson);

    }

    public class Person
    { 
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public City? City { get; set; } 
        public Resume? Resume { get; set; }
        public Person()
        {
            Id = Guid.NewGuid();

        }

    }

    public class Resume
    {
        public Guid Id { get; init; }
        public string? Description { get; set; }
        public int? Salary { get; set; }
        public List<Employment> Employment { get; set; }
        public Resume()
        {
            Id = Guid.NewGuid();
        }
    }

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
}