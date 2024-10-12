using System.ComponentModel.DataAnnotations;

namespace PatientManagment.Models.QueryModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required..")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
