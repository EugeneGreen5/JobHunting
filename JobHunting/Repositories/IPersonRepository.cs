using JobHunting.Models.DTO;
using JobHunting.Models.Entity;

namespace JobHunting.Repositories;

public interface IPersonRepository
{
    Task AddPersonAsync(Person newPerson);
    Task<Person> GetPersonByIdAsync(Guid id);
    Task<IList<Person>> GetPersonsAsync();
    Task<bool> DeleteByIdAsync(Guid id);
    Task UpdateByPerson(Person person, PersonDTO newPerson);
    Task<Person?> GetPersonForAuth(string name, string password);
    Task ChangeRole(Guid id);
}
