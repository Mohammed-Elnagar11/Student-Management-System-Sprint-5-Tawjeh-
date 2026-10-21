using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Dtos;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;

namespace Student_Management_System__Sprint_5_Tawjeh_.Services
{
    public enum CourseDeleteResult
    {
        Success,
        NotFound,
        HasEnrollments
    }

    public class CourseService
    {
        private readonly AppDbContext context;

        public CourseService(AppDbContext context)
        {
            this.context = context;
        }

        // READ : Display-only reads use AsNoTracking()
        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> CourseExistsAsync(int id)
        {
            return await context.Courses
                .AsNoTracking()
                .AnyAsync(c => c.Id == id);
        }

        // CREATE
        public async Task<Course> AddCourseAsync(Course course)
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> HasEnrollmentsAsync(int courseId)
        {
            return await context.Enrollments
                .AsNoTracking()
                .AnyAsync(e => e.CourseId == courseId);
        }

        // DELETE : Delete behavior is Restrict: cannot delete a course with enrollments
        public async Task<CourseDeleteResult> DeleteCourseAsync(int id)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return CourseDeleteResult.NotFound;
            }

            var hasEnrollments = await HasEnrollmentsAsync(id);
            if (hasEnrollments)
            {
                return CourseDeleteResult.HasEnrollments;
            }

            try
            {
                context.Courses.Remove(course);
                await context.SaveChangesAsync();
                return CourseDeleteResult.Success;
            }
            catch (DbUpdateException)
            {
                return CourseDeleteResult.HasEnrollments;
            }
        }

        // RELATED DATA : Single projected query prevents N+1 problem
        public async Task<List<CourseStudentResponse>?> GetStudentsInCourseAsync(int courseId)
        {
            var exists = await CourseExistsAsync(courseId);
            if (!exists)
            {
                return null;
            }

            return await context.Enrollments
                .AsNoTracking()
                .Where(e => e.CourseId == courseId)
                .Select(e => new CourseStudentResponse
                {
                    StudentId = e.StudentId,
                    FullName = e.Student.FullName,
                    Email = e.Student.Email,
                    Grade = e.Grade
                })
                .ToListAsync();
        }

        // AGGREGATION : Average grade ignores null grades
        public async Task<(bool CourseExists, double? AverageGrade)> GetAverageGradeForCourseAsync(int courseId)
        {
            var exists = await CourseExistsAsync(courseId);
            if (!exists)
            {
                return (false, null);
            }

            var gradedEnrollments = context.Enrollments
                .AsNoTracking()
                .Where(e => e.CourseId == courseId && e.Grade.HasValue);

            var count = await gradedEnrollments.CountAsync();
            if (count == 0)
            {
                return (true, null);
            }

            var average = await gradedEnrollments.AverageAsync(e => (double)e.Grade!.Value);
            return (true, average);
        }
    }

}