using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Entities
{
    internal class Instructor
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
