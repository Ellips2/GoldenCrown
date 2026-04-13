using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs
{
    public class RegisterRequest
    {
        [Required (ErrorMessage = "Field Login is Required")]
        [MinLength(3, ErrorMessage = "Minimal login length is 3 chars")]
        public string Login { get; set; }

        [Required (ErrorMessage = "Field Name is Required", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required (ErrorMessage = "Field Password is Required")]
        [MinLength(6, ErrorMessage = "Minimal password length is 6 chars")]
        public string Password { get; set; }
    }
}
