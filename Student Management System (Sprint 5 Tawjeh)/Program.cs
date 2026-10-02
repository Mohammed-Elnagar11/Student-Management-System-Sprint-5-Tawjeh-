using System;
using System.Threading.Tasks;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Services;

namespace Student_Management_System__Sprint_5_Tawjeh_
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new AppDbContext();
            
            var studentService = new StudentService(context);
            var courseService = new CourseService(context);
            var enrollmentService = new EnrollmentService(context);

            Console.WriteLine("--- Student Management System ---");
            Console.WriteLine();

            // Search for a student
            Console.WriteLine("1. Search Student by Name ('Ali'):");
            var searchResults = await studentService.SearchStudentsByNameAsync("Ali");
            foreach (var s in searchResults)
            {
                Console.WriteLine($"- Found: {s.FullName} ({s.Email})");
            }
            Console.WriteLine();

            // Get Student with Enrollments (Testing Include)
            Console.WriteLine("2. Get Student with Enrollments (Student ID: 1):");
            var studentWithEnrollments = await studentService.GetStudentWithEnrollmentsAsync(1);
            if (studentWithEnrollments != null)
            {
                Console.WriteLine($"Student: {studentWithEnrollments.FullName}");
                foreach (var e in studentWithEnrollments.Enrollments)
                {
                    Console.WriteLine($"  - Enrolled in: {e.Course.Title}, Grade: {e.Grade?.ToString() ?? "N/A"}");
                }
            }
            Console.WriteLine();

            // Get Students in a Course
            Console.WriteLine("3. Get Students in Course ID 1:");
            var studentsInCourse = await enrollmentService.GetStudentsInCourseAsync(1);
            foreach (var s in studentsInCourse)
            {
                Console.WriteLine($"  - {s.FullName}");
            }
            Console.WriteLine();

            // Get Average Grade for a Course
            Console.WriteLine("4. Get Average Grade for Course ID 2:");
            var avgGrade = await enrollmentService.GetAverageGradeForCourseAsync(2);
            Console.WriteLine($"  - Average Grade: {avgGrade?.ToString("F2") ?? "No grades yet"}");
            Console.WriteLine();

            // Test Soft Delete
            Console.WriteLine("5. Testing Soft Delete on Student ID 4 (Nour Hassan):");
            var softDeleteSuccess = await studentService.SoftDeleteStudentAsync(4);
            Console.WriteLine($"  - Soft Delete Successful: {softDeleteSuccess}");
            
            Console.WriteLine("  - Fetching Deleted Students:");
            var deletedStudents = await studentService.GetDeletedStudentsAsync();
            foreach (var ds in deletedStudents)
            {
                Console.WriteLine($"    - Deleted: {ds.FullName}");
            }
            Console.WriteLine();

            // Test Restrict Delete Behavior on Course
            Console.WriteLine("6. Testing Restrict Delete Behavior on Course ID 1 (Has Enrollments):");
            try
            {
                await courseService.DeleteCourseAsync(1);
                Console.WriteLine("  - Course deleted successfully (This should not happen).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  - Delete failed as expected due to Restrict behavior.");
                Console.WriteLine($"  - Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Testing complete.");
        }
    }
}
