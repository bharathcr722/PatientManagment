﻿using System.ComponentModel.DataAnnotations;

namespace PatientManagment.Models
{
    public class PatientDataModel
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
        
        public string Password { get; set; }
    }
}
