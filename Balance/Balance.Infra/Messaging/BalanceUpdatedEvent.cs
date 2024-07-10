namespace Balance.Infra.Messaging;

public class BalanceUpdatedEvent
{
    public string AccountId { get; set; }
    public double Amount { get; set; }
    public DateTime EventDate { get; set; }
}