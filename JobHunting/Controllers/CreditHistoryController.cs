/*using JobHunting.Models.DTO;
using JobHunting.Models.Entity;
using JobHunting.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditHistoryController : ControllerBase
{
    private readonly ICreditHistoryRepository _creditHistoryRepository;
    private readonly IPersonRepository _personRepository;

    public CreditHistoryController(ICreditHistoryRepository creditHistoryRepository,
        IPersonRepository personRepository)
    {
        _creditHistoryRepository = creditHistoryRepository;
        _personRepository = personRepository;
    }

    [HttpGet("create")]
    public async Task CreateTable()
    {
        await _creditHistoryRepository.CreateTable();
    }

    [HttpGet("drop")]
    public async Task DropTable()
    {
        await _creditHistoryRepository.DropTable();
    }

    [HttpGet("delete")]
    public async Task DeleteTable()
    {
        await _creditHistoryRepository.DeleteTable();
    }

    [HttpPost]
    public async Task AddValue(CreditHistoryDTO dto)
    {
        var currentAmount = await CalculateCurrentSum(dto.UserId);

        var entity = new CreditHistoryEntity
        {
            Amount = currentAmount + dto.Change,
            UserId = dto.UserId,
            Change = dto.Change
        };

        await _creditHistoryRepository.AddValue(entity);
    }

    [HttpGet("history/all")]
    public async Task<IEnumerable<CreditHistoryEntity>> GetAllHistories() => 
        await _creditHistoryRepository.GetAllHistories();

    [HttpGet("history/{id}")]
    public async Task<IEnumerable<CreditHistoryEntity>> GetHistoriesById(Guid id) =>
        await _creditHistoryRepository.GetAllHistoriesById(id);

    private async Task<int> CalculateCurrentSum(Guid id)
    {
        var lastOperation = await _creditHistoryRepository.GetAllHistoriesById(id);
        if (lastOperation.Count() == 0) return 0;
        return lastOperation.Last().Amount;
    }
}
*/