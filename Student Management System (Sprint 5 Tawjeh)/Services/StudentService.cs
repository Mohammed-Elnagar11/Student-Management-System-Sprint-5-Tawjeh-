using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Dtos;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;

namespace Student_Management_System__Sprint_5_Tawjeh_.Services
{
    public class StudentService
    {
        private readonly AppDbContext context;
        public StudentService(AppDbContext context)
        {
            this.context = context;
        }

        // READ : Display-only reads use AsNoTracking()
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await context.Students
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Student>> SearchStudentsByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await GetAllStudentsAsync();
            }

            return await context.Students
                .AsNoTracking()
                .Where(s => s.FullName.Contains(name))
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await context.Students
                .AsNoTracking()
                .AnyAsync(s => s.Email == email && (!excludeId.HasValue || s.Id != excludeId.Value));
        }

        public async Task<bool> StudentExistsAsync(int id)
        {
            return await context.Students
                .AsNoTracking()
                .AnyAsync(s => s.Id == id);
        }

        // CREATE
        public async Task<Student> AddStudentAsync(Student student)
        {
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student;
        }

        // UPDATE : No AsNoTracking() so the change tracker can track modifications
        public async Task<Student?> UpdateStudentAsync(int id, string fullName, string email, DateTime dateOfBirth)
        {
            var student = await context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return null;
            }

            student.FullName = fullName;
            student.Email = email;
            student.DateOfBirth = dateOfBirth;

            await context.SaveChangesAsync();
            return student;
        }

        // DELETE - Physical delete triggers cascade delete on Enrollments
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return false;
            }

            context.Students.Remove(student);
            await context.SaveChangesAsync();
            return true;
        }

        // RELATED DATA - Single projected query prevents N+1 problem
        public async Task<List<StudentCourseResponse>?> GetCoursesForStudentAsync(int studentId)
        {
            var studentExists = await StudentExistsAsync(studentId);
            if (!studentExists)
            {
                return null;
            }

            return await context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Select(e => new StudentCourseResponse
                {
                    CourseId = e.CourseId,
                    Title = e.Course.Title,
                    Credits = e.Course.Credits,
                    Grade = e.Grade
                })
                .ToListAsync();
        }
    }
}
