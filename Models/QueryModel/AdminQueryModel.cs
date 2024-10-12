using System.ComponentModel.DataAnnotations;

namespace PatientManagment.Models.QueryModel
{
    public class AdminQueryModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
        [Required]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
