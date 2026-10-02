using Microsoft.EntityFrameworkCore;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Services
{
    internal class CourseService
    {
        private readonly AppDbContext context;

        public CourseService(AppDbContext context)
        {
            this.context = context;
        }

        // Create
        public async Task<Course> AddCourseAsync(Course course)
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            return course;
        }

        // Read
        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await context.Courses
                .AsNoTracking()
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await context.Courses
                .AsNoTracking()
                .Include(c => c.Instructor)
                .ToListAsync();
        }

        // UPDATE

        public async Task<Course?> UpdateCourseAsync(int courseId, string title, int credits, string? description)
        {
            // No AsNoTracking — we need the tracker to detect changes
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
             return null;

            course.Title = title;
            course.Credits = credits;
            course.Description = description;

            await context.SaveChangesAsync();
            return course;
        }

        // DELETE
        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course is null) return false;

            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return true;
        }
    }

}