
using System.ComponentModel.DataAnnotations;

namespace PatientManagment.Data
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(4)]
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }

    }
}
