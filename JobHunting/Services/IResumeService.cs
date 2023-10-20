using JobHunting.Models.DTO;
using JobHunting.Models.Entity;

namespace JobHunting.Services;

public interface IResumeService
{
    Task<ExtensionDTO> AddResumeAsync(Guid id, Resume resume);
    Task<ExtensionDTO> DeleteResumeAsync(Guid idPerson, Guid idResume);
}
