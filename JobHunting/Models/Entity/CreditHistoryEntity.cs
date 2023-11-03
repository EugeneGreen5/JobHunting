namespace JobHunting.Models.Entity;

public class CreditHistoryEntity
{
    public int Id { get; set; }
    public Guid UserId { get; init; }
    public int Change { get; set; }
    public int Amount { get; set; }
}
