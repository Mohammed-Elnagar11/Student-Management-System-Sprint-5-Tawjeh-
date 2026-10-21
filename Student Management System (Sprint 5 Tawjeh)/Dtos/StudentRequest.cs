using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System__Sprint_5_Tawjeh_.Dtos
{
    public class StudentRequest
    {
        [Required(ErrorMessage = "FullName is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 150 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DateOfBirth { get; set; }

        public DateTime? EnrollmentDate { get; set; }
    }
}
