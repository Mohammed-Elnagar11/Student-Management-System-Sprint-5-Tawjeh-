using Microsoft.EntityFrameworkCore;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;

namespace Student_Management_System__Sprint_5_Tawjeh_.Services
{
    internal class EnrollmentService
    {
        private readonly AppDbContext context;

        public EnrollmentService(AppDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
        {
            context.Enrollments.Add(enrollment);
            await context.SaveChangesAsync();
            return enrollment;
        }

        // READ
        public async Task<Enrollment?> GetEnrollmentByIdAsync(int enrollmentId)
        {
            return await context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsInCourseAsync(int courseId)
        {
            return await context.Enrollments
                .AsNoTracking()
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student)
                .Where(s => s != null)
                .ToListAsync();
        }

        public async Task<List<(Course Course, int? Grade)>> GetCoursesForStudentAsync(int studentId)
        {
            var rows = await context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Select(e => new { e.Course, e.Grade })
                .ToListAsync();

            return rows.Select(x => (x.Course, x.Grade)).ToList();
        }

        public async Task<double?> GetAverageGradeForCourseAsync(int courseId)
        {
            var grades = await context.Enrollments
                .AsNoTracking()
                .Where(e => e.CourseId == courseId && e.Grade.HasValue)
                .Select(e => (double)e.Grade!.Value)
                .ToListAsync();

            return grades.Count == 0 ? null : grades.Average();
        }

        // UPDATE
        public async Task<Enrollment?> UpdateEnrollmentGradeAsync(int enrollmentId, int? grade)
        {
            var enrollment = await context.Enrollments.FirstOrDefaultAsync(e => e.Id == enrollmentId);
            if (enrollment is null) return null;

            enrollment.Grade = grade;
            await context.SaveChangesAsync();
            return enrollment;
        }

        // DELETE
        public async Task<bool> DeleteEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await context.Enrollments.FirstOrDefaultAsync(e => e.Id == enrollmentId);
            if (enrollment is null) return false;

            context.Enrollments.Remove(enrollment);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
