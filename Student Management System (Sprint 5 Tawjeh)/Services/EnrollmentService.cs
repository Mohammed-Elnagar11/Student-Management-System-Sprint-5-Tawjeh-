using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;

namespace Student_Management_System__Sprint_5_Tawjeh_.Services
{
    public enum EnrollmentStatus
    {
        Success,
        NotFound,
        Duplicate
    }

    public class EnrollmentCreateResult
    {
        public EnrollmentStatus Status { get; set; }
        public string? Message { get; set; }
        public Enrollment? Enrollment { get; set; }
    }

    public class EnrollmentService
    {
        private readonly AppDbContext context;

        public EnrollmentService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> StudentExistsAsync(int studentId)
        {
            return await context.Students
                .AsNoTracking()
                .AnyAsync(s => s.Id == studentId);
        }

        public async Task<bool> CourseExistsAsync(int courseId)
        {
            return await context.Courses
                .AsNoTracking()
                .AnyAsync(c => c.Id == courseId);
        }

        public async Task<bool> EnrollmentExistsAsync(int studentId, int courseId)
        {
            return await context.Enrollments
                .AsNoTracking()
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<EnrollmentCreateResult> AddEnrollmentAsync(Enrollment enrollment)
        {
            var studentExists = await StudentExistsAsync(enrollment.StudentId);
            if (!studentExists)
            {
                return new EnrollmentCreateResult
                {
                    Status = EnrollmentStatus.NotFound,
                    Message = $"Student with ID {enrollment.StudentId} does not exist."
                };
            }

            var courseExists = await CourseExistsAsync(enrollment.CourseId);
            if (!courseExists)
            {
                return new EnrollmentCreateResult
                {
                    Status = EnrollmentStatus.NotFound,
                    Message = $"Course with ID {enrollment.CourseId} does not exist."
                };
            }

            var isDuplicate = await EnrollmentExistsAsync(enrollment.StudentId, enrollment.CourseId);
            if (isDuplicate)
            {
                return new EnrollmentCreateResult
                {
                    Status = EnrollmentStatus.Duplicate,
                    Message = $"Student {enrollment.StudentId} is already enrolled in Course {enrollment.CourseId}."
                };
            }

            try
            {
                context.Enrollments.Add(enrollment);
                await context.SaveChangesAsync();
                return new EnrollmentCreateResult
                {
                    Status = EnrollmentStatus.Success,
                    Enrollment = enrollment
                };
            }
            catch (DbUpdateException)
            {
                return new EnrollmentCreateResult
                {
                    Status = EnrollmentStatus.Duplicate,
                    Message = "A duplicate enrollment record already exists."
                };
            }
        }
    }
}
