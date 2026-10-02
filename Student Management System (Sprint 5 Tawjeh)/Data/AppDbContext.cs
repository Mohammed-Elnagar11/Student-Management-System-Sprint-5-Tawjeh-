using Microsoft.EntityFrameworkCore;
using Student_Management_System__Sprint_5_Tawjeh_.Data.Configurations;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Data
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-C6238S4\\SQLEXPRESS;Database=StudentSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;");
        }
        // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());
            modelBuilder.ApplyConfiguration(new InstructorConfiguration());

            // === Seed data ===

            // Instructors
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, FullName = "Dr. Sarah Johnson" },
                new Instructor { Id = 2, FullName = "Prof. Ahmed Hassan" },
                new Instructor { Id = 3, FullName = "Dr. Emily Chen" }
            );

            // Courses (3+ courses)
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "Introduction to Computer Science", Credits = 3, Description = "Fundamental concepts of programming and computational thinking.", InstructorId = 1 },
                new Course { Id = 2, Title = "Database Systems", Credits = 4, Description = "Relational database design, SQL, and modern ORM frameworks.", InstructorId = 2 },
                new Course { Id = 3, Title = "Data Structures and Algorithms", Credits = 4, Description = "Core data structures: lists, trees, graphs, and algorithm analysis.", InstructorId = 1 },
                new Course { Id = 4, Title = "Web Development", Credits = 3, Description = "Frontend and backend web development with modern frameworks.", InstructorId = 3 }
            );

            // Students (3+ students)
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FullName = "Ali Mohamed",
                    Email = "ali.mohamed@university.edu",
                    DateOfBirth = new DateTime(2000, 5, 14),
                    EnrollmentDate = new DateTime(2022, 9, 1),
                    IsDeleted = false
                },
                new Student
                {
                    Id = 2,
                    FullName = "Sara Ahmed",
                    Email = "sara.ahmed@university.edu",
                    DateOfBirth = new DateTime(2001, 3, 22),
                    EnrollmentDate = new DateTime(2022, 9, 1),
                    IsDeleted = false
                },
                new Student
                {
                    Id = 3,
                    FullName = "Omar Khaled",
                    Email = "omar.khaled@university.edu",
                    DateOfBirth = new DateTime(1999, 11, 8),
                    EnrollmentDate = new DateTime(2021, 9, 1),
                    IsDeleted = false
                },
                new Student
                {
                    Id = 4,
                    FullName = "Nour Hassan",
                    Email = "nour.hassan@university.edu",
                    DateOfBirth = new DateTime(2002, 7, 17),
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    IsDeleted = false
                }
            );

            // Enrollments (each student in multiple courses)
            modelBuilder.Entity<Enrollment>().HasData(
                // Ali: CS + DB + DSA
                new Enrollment { Id = 1, StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2022, 9, 5), Grade = 88 },
                new Enrollment { Id = 2, StudentId = 1, CourseId = 2, EnrollmentDate = new DateTime(2022, 9, 5), Grade = 92 },
                new Enrollment { Id = 3, StudentId = 1, CourseId = 3, EnrollmentDate = new DateTime(2023, 2, 1), Grade = null },
                // Sara: CS + Web
                new Enrollment { Id = 4, StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2022, 9, 5), Grade = 76 },
                new Enrollment { Id = 5, StudentId = 2, CourseId = 4, EnrollmentDate = new DateTime(2023, 2, 1), Grade = 85 },
                // Omar: DB + DSA + Web
                new Enrollment { Id = 6, StudentId = 3, CourseId = 2, EnrollmentDate = new DateTime(2021, 9, 5), Grade = 95 },
                new Enrollment { Id = 7, StudentId = 3, CourseId = 3, EnrollmentDate = new DateTime(2021, 9, 5), Grade = 78 },
                new Enrollment { Id = 8, StudentId = 3, CourseId = 4, EnrollmentDate = new DateTime(2022, 2, 1), Grade = 90 },
                // Nour: CS + DB
                new Enrollment { Id = 9, StudentId = 4, CourseId = 1, EnrollmentDate = new DateTime(2023, 9, 5), Grade = null },
                new Enrollment { Id = 10, StudentId = 4, CourseId = 2, EnrollmentDate = new DateTime(2023, 9, 5), Grade = null }
            );
        }
    }
}
