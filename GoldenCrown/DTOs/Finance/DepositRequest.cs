using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Finance
{
    public class DepositRequest
    {
        [Required(ErrorMessage = "Field Token is Required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Field Amount is Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal Amount { get; set; }
    }
}
