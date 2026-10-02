using Microsoft.EntityFrameworkCore;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Services
{
    internal class StudentService
    {
        private readonly AppDbContext context;
        public StudentService(AppDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public async Task<Student> AddStudentAsync(Student student)
        {
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student;
        }

        // READ
        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            return await context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await context.Students
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Student>> SearchStudentsByNameAsync(string name)
        {
            return await context.Students
                .AsNoTracking()
                .Where(s => s.FullName.Contains(name))
                .ToListAsync();
        }

        public async Task<Student?> GetStudentWithEnrollmentsAsync(int studentId)
        {
            return await context.Students
                .AsNoTracking()
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        // UPDATE
        public async Task<Student?> UpdateStudentAsync(int studentId, string fullName, string email, DateTime dateOfBirth)
        {
            // No AsNoTracking here — we need the tracker to detect changes
            var student = await context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student is null) return null;

            student.FullName = fullName;
            student.Email = email;
            student.DateOfBirth = dateOfBirth;

            await context.SaveChangesAsync();
            return student;
        }

        // DELETE
        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = await context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student is null) return false;

            context.Students.Remove(student);
            await context.SaveChangesAsync();
            return true;
        }

        // SOFT DELETE (Bonus)
        public async Task<bool> SoftDeleteStudentAsync(int studentId)
        {
            // IgnoreQueryFilters needed here: the student might already appear deleted
            // OR we just look them up normally (still active at this point)
            var student = await context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);
            if (student is null) return false;

            student.IsDeleted = true;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetDeletedStudentsAsync()
        {
            return await context.Students
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(s => s.IsDeleted)
                .ToListAsync();
        }
    }
}
