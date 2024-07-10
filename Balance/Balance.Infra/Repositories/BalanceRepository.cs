using Balance.Domain.Interfaces;
using Balance.Infra.Data;

namespace Balance.Infra.Repositories;

public class BalanceRepository(ApplicationDbContext context) : IBalanceRepository
{
    private readonly ApplicationDbContext _context = context;

    public Domain.Entities.Balance GetByAccountId(string accountId) 
        => _context.Balances.FirstOrDefault(b => b.AccountId == accountId);

    public void UpdateBalance(Domain.Entities.Balance balance)
    {
        _context.Balances.Update(balance);
        _context.SaveChanges();
    }
}
