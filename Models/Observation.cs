using System.ComponentModel.DataAnnotations;

namespace PatientManagment.Models
{
    public class Observation
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string Note { get; set; }

        public DateTime Date { get; set; }

        public Patient Patient { get; set; }
    }
}
