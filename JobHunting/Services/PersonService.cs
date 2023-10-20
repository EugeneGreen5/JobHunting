using JobHunting.Data;
using JobHunting.Models.DTO;
using JobHunting.Models.Entity;
using JobHunting.Repositories;

namespace JobHunting.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<ExtensionDTO> AddPersonAsync(Person newPerson)
    {

        if (newPerson.Resumes is null) newPerson.Resumes = new List<Resume>();

        await _personRepository.AddPersonAsync(newPerson);
        return new ExtensionDTO
        {
            Code = 201,
            Information = "Удачно добавлено"
        };
    }
    public async Task<ExtensionDTO> DeletePersonByIdAsync(Guid id)
    {
        var currentPerson = await _personRepository.DeleteByIdAsync(id);
        if (currentPerson is false) return new ExtensionDTO { Code = 404, Information = "Не найдено" };

        return new ExtensionDTO { Code = 204, Information = "Удалено"};
    }

    public async Task<IList<Person>> GetAllPersonsAsync() =>
        await _personRepository.GetPersonsAsync();

    public async Task<(ExtensionDTO, Person)> GetPersonByIdAsync(Guid id)
    {
        var currentPerson = await _personRepository.GetPersonByIdAsync(id);
        return (new ExtensionDTO { Code = 200, Information = "Объект"}, currentPerson);
    }

    public async Task<ExtensionDTO> UpdatePersonByIdAsync(Guid id, PersonDTO newPerson)
    {
        var currentPerson = await _personRepository.GetPersonByIdAsync(id);
        if (currentPerson is null || newPerson is null) return new ExtensionDTO { Code = 404, Information = "Не найдено"};

        _personRepository.UpdateByPerson(currentPerson, newPerson);

        return new ExtensionDTO { Code = 200, Information = "Обновлено" };
    }
        
}
