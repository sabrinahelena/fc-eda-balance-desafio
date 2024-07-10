using Balance.Domain.Interfaces;

namespace Balance.Application.Services;

public class BalanceService(IBalanceRepository balanceRepository)
{
    private readonly IBalanceRepository _balanceRepository = balanceRepository;

    public Domain.Entities.Balance GetBalance(string accountId)
    {
        return _balanceRepository.GetByAccountId(accountId);
    }
}