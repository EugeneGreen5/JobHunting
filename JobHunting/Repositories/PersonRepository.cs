using JobHunting.Data;
using JobHunting.Models.DTO;
using JobHunting.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _context;

    public PersonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddPersonAsync(Person newPerson)
    {
        await _context.Persons.AddAsync(newPerson);
        await _context.SaveChangesAsync();
    }

    public async Task<Person> GetPersonByIdAsync(Guid id) =>
         _context.Persons.Include(x => x.Resumes).FirstOrDefault(x => x.Id == id);

    public async Task<IList<Person>> GetPersonsAsync() =>
        _context.Persons.Include(x => x.Resumes).ToList();

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var person = await GetPersonByIdAsync(id);
        if (person is null) return false;

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task UpdateByPerson(Person person, PersonDTO newPerson)
    {
        (person.Name, person.Email, person.Phone, person.City)
            = (newPerson.Name, newPerson.Email, newPerson.Phone, newPerson.City);

        await _context.SaveChangesAsync();
    }
}
