using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    internal class Student
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
