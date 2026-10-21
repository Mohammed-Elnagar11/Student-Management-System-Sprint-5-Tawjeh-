namespace Student_Management_System__Sprint_5_Tawjeh_.Dtos
{
    public class CourseStudentResponse
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? Grade { get; set; }
    }
}
