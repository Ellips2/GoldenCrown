using Microsoft.AspNetCore.Mvc;

namespace GoldenCrown.DTOs.Finance
{
    public class BalanceRequest
    {
        [FromQuery] public string Currency { get; set; }
    }
}
