using System.ComponentModel.DataAnnotations;

namespace PatientManagment.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        [MinLength(4)]
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public virtual ICollection<Observation> Observations { get; set; }
    }
}
