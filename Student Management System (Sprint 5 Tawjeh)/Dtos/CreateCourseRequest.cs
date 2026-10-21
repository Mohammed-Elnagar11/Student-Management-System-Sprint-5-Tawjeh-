using System.ComponentModel.DataAnnotations;

namespace Student_Management_System__Sprint_5_Tawjeh_.Dtos
{
    public class CreateCourseRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Credits is required.")]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")]
        public int Credits { get; set; }

        [MaxLength(750, ErrorMessage = "Description cannot exceed 750 characters.")]
        public string? Description { get; set; }
    }
}
