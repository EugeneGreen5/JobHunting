using JobHunting.Data;
using JobHunting.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Repositories;

public class ResumeRepository : IResumeRepository
{
    private readonly ApplicationDbContext _context;

    public ResumeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddResumeAsync(Resume newResume)
    {
        await _context.Resumes.AddAsync(newResume);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveResumeAsync(Resume newResume)
    {
        _context.Resumes.Remove(newResume);
        await _context.SaveChangesAsync();
    }
}
