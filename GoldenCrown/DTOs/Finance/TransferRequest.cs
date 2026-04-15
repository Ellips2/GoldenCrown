using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Finance
{
    public class TransferRequest
    {
        [FromQuery]
        [Required(ErrorMessage = "Field Token is Required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Field ReceiverLogin is Required")]
        [MinLength(3, ErrorMessage = "Minimal login length is 3 chars")]
        public string ReceiverLogin { get; set; }

        [Required(ErrorMessage = "Field Amount is Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal Amount { get; set; }
    }
}
