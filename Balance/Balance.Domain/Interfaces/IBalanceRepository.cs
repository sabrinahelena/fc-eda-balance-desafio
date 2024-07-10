namespace Balance.Domain.Interfaces;

public interface IBalanceRepository
{
    Domain.Entities.Balance GetByAccountId(string accountId);
    void UpdateBalance(Domain.Entities.Balance balance);
}
