using JobHunting.Models.Entity;

namespace JobHunting.Repositories;

public interface ICreditHistoryRepository
{
    Task CreateTable();
    Task DropTable();
    Task DeleteTable();
    Task AddValue(CreditHistoryEntity creditHistory);
    Task<IEnumerable<CreditHistoryEntity>> GetAllHistories();
    Task<IEnumerable<CreditHistoryEntity>> GetAllHistoriesById(Guid id);
}
