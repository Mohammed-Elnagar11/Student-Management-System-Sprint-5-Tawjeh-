using System;
using System.Collections.Generic;

namespace Student_Management_System__Sprint_5_Tawjeh_.Entities
{
    internal class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
