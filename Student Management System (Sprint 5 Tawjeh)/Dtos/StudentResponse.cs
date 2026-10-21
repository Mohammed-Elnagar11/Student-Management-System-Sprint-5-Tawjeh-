using System;

namespace Student_Management_System__Sprint_5_Tawjeh_.Dtos
{
    public class StudentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
