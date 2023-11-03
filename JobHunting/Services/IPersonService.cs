using JobHunting.Models.DTO;
using JobHunting.Models.Entity;

namespace JobHunting.Services;

public interface IPersonService
{
    Task<ExtensionDTO> AddPersonAsync(Person newPerson);
    Task<ExtensionDTO> DeletePersonByIdAsync(Guid id);
    Task<IList<Person>> GetAllPersonsAsync();
    Task<(ExtensionDTO, Person)> GetPersonByIdAsync(Guid id);
    Task<ExtensionDTO> UpdatePersonByIdAsync(Guid id, PersonDTO newPerson);
    Task<Person> GetPersonForAuth(String name, string password);
}
