using JobHunting.Models.Entity;

namespace JobHunting.Repositories;

public interface IResumeRepository
{
    Task AddResumeAsync(Resume newResume);
    Task RemoveResumeAsync(Resume newResume);
}
