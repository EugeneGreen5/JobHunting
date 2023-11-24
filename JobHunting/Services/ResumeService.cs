using JobHunting.Models.DTO;
using JobHunting.Models.Entity;
using JobHunting.Repositories;

namespace JobHunting.Services;

public class ResumeService : IResumeService
{
    private readonly IResumeRepository _resumeRepository;
    private readonly IPersonRepository _personRepository;

    public ResumeService(IResumeRepository resumeRepository, IPersonRepository personRepository)
    {
        _resumeRepository = resumeRepository;
        _personRepository = personRepository;
    }

    public async Task<ExtensionDTO> AddResumeAsync(Guid id, Resume resume)
    {
        var currentPerson = await _personRepository.GetPersonByIdAsync(id);
        if (currentPerson is null || resume is null) return new ExtensionDTO { Code = 404, Information = "Не найдено" };

        resume.PersonId = id;
        await _resumeRepository.AddResumeAsync(resume);
        return new ExtensionDTO { Code = 200, Information = "Добавлено" };
    }

    public async Task<ExtensionDTO> DeleteResumeAsync(Guid idPerson, Guid idResume)
    {
        var currentPerson = await _personRepository.GetPersonByIdAsync(idPerson);
        var currentResume = currentPerson.Resumes.FirstOrDefault(p => p.Id == idResume);
        if (currentPerson is null || currentResume is null) return new ExtensionDTO { Code = 404, Information = "Не найдено" };

        await _resumeRepository.RemoveResumeAsync(currentResume);
        return new ExtensionDTO { Code = 204, Information = "Удалено" };
    }
}
