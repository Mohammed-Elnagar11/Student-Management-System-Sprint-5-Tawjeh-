using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Entities
{
    [PrimaryKey(nameof(StudentId), nameof(CourseId))]
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
