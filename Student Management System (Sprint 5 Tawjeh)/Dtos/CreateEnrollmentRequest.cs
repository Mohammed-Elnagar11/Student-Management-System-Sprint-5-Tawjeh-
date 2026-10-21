using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System__Sprint_5_Tawjeh_.Dtos
{
    public class CreateEnrollmentRequest
    {
        [Required(ErrorMessage = "StudentId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive integer.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "CourseId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a positive integer.")]
        public int CourseId { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public int? Grade { get; set; }
    }
}
