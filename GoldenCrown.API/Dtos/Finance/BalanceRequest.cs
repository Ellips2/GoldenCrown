using Microsoft.AspNetCore.Mvc;

namespace GoldenCrown.API.Dtos.Finance
{
    public class BalanceRequest
    {
        [FromQuery] public string Currency { get; set; }
    }
}
