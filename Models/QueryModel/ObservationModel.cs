using System.ComponentModel.DataAnnotations;

namespace PatientManagment.Models.QueryModel
{
    public class ObservationModel
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string Note { get; set; }

        public DateTime Date { get; set; }
    }
}
