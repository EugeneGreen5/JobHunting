using Dapper;
using JobHunting.Models.Entity;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobHunting.Repositories;

public class CreditHistoryRepository : ICreditHistoryRepository
{
    private const string connString = "Server=(localdb)\\mssqllocaldb;Database=JobHunting;";

    public async Task CreateTable()
    {
        var query = @"create table credit_history
            (
	            id INT IDENTITY,
	            user_id uniqueidentifier NOT NULL,
	            change int NOT NULL,
	            amount int NOT NULL
            );";
        using IDbConnection db = new SqlConnection(connString);
        await db.ExecuteAsync(query);
    }

    public async Task DropTable()
    {
        var query = "DROP TABLE credit_history;";
        using IDbConnection db = new SqlConnection(connString);
        await db.ExecuteAsync(query);
    }

    public async Task DeleteTable()
    {
        var query = "DELETE FROM credit_history;";
        using IDbConnection db = new SqlConnection(connString);
        await db.ExecuteAsync(query);
    }

    public async Task AddValue(CreditHistoryEntity creditHistory)
    {
        var query = @"INSERT INTO credit_history (user_id, change, amount)
            VALUES (@UserId, @Change, @Amount);";
        using IDbConnection db = new SqlConnection(connString);
        await db.ExecuteAsync(query, creditHistory);
    }

    public async Task<IEnumerable<CreditHistoryEntity>> GetAllHistories()
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<CreditHistoryEntity>("SELECT id, change, amount, user_id AS UserId FROM credit_history;");
    }

    public async Task<IEnumerable<CreditHistoryEntity>> GetAllHistoriesById(Guid id)
    {
        using IDbConnection db = new SqlConnection(connString);
        var a = new { id };
        return db.Query<CreditHistoryEntity>(@"SELECT * FROM credit_history WHERE user_id = @id;",new { id } );
    }
}
