namespace Student_Management_System__Sprint_5_Tawjeh_.Dtos
{
    public class StudentCourseResponse
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int? Grade { get; set; }
    }
}
