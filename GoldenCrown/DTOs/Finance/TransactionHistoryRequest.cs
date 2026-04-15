using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Finance
{
    public class TransactionHistoryRequest
    {
        [FromQuery]
        [Required(ErrorMessage = "Field Token is Required")]
        public string Token { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Limit must be positive")]
        public int? Limit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Offset must be non-negative")]
        public int? Offset { get; set; }
    }
}
