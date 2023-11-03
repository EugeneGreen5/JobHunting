/*using JobHunting.Models.Entity;
using JobHunting.Models;

namespace JobHunting.Data;

public class DB
{
    public static IList<Person> MockData()
    {
        Resume firstResume = new()
        {
            Description = "123",
            Salary = 230000,
            IsFullEmployment = true,
            IsPartTimeEmployment = true,
            IsTrainee = true
        };
        Resume secondResume = new()
        {
            Description = "456",
            Salary = 25000,
            IsTrainee = true

        };
        Resume thirdResume = new Resume()
        {
            Description = "789",
            Salary = 90020210,
            IsFullEmployment = true,
            IsVolunteering = true,

        };

        Person firstPerson = new Person()
        {
            Name = "Корнеев Руслан Романович",
            Email = "fyodor70@yahoo.com",
            Phone = "+7(448)138-76-12",
            City = City.Moscow,
            Resumes = new List<Resume>()
            {
                thirdResume,
                firstResume
            }
        };
        Person secondPerson = new Person()
        {
            Name = "Баранова Екатерина Ивановна",
            Email = "zhanna_efimova@yandex.ru",
            Phone = "+7(918)459-00-38",
            City = City.Samara,
            Resumes = new List<Resume>()
            {
                secondResume
            }
        };
        Person thirdPerson = new Person()
        {
            Name = "Михайлов Даниил Гордеевич",
            Email = "lyudmila_yudina@mail.ru",
            Phone = "+7(582)948-84-85",
            City = City.Saratov,
            Resumes = new List<Resume>()
            {
                thirdResume
            }
        };

        *//*resumes.Add(firstResume);
        resumes.Add(secondResume);
        resumes.Add(thirdResume);*//*

        return new List<Person> { firstPerson, secondPerson, thirdPerson };

    }
}
*/