using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Entities
{
    internal class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;
        [ForeignKey("Instructor")]
        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
