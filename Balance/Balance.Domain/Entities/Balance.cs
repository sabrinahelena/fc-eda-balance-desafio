namespace Balance.Domain.Entities;

public class Balance
{
    public string Id { get; set; }
    public string AccountId { get; set; }
    public double Amount { get; set; }
    public DateTime LastUpdated { get; set; }
}