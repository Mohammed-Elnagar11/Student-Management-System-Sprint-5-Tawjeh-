using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System__Sprint_5_Tawjeh_.Entities
{
    internal class Enrollment
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int? Grade { get; set; }
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
