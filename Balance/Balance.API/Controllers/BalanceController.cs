using Balance.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Balance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BalanceController : ControllerBase
{
    private readonly BalanceService _balanceService;

    public BalanceController(BalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    [HttpGet("{accountId}")]
    public IActionResult GetBalance(string accountId)
    {
        var balance = _balanceService.GetBalance(accountId);
        if (balance == null)
        {
            return NotFound();
        }
        return Ok(balance);
    }
}