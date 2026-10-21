using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Management_System__Sprint_5_Tawjeh_.Dtos;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;
using Student_Management_System__Sprint_5_Tawjeh_.Services;

namespace Student_Management_System__Sprint_5_Tawjeh_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: /api/students
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();
            var response = students.Select(MapToResponse).ToList();
            return Ok(response);
        }

        // GET: /api/students/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentResponse>> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"Student with ID {id} was not found."
                });
            }

            return Ok(MapToResponse(student));
        }

        // GET: /api/students/search?name=
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentResponse>>> Search([FromQuery] string? name)
        {
            var students = await _studentService.SearchStudentsByNameAsync(name);
            var response = students.Select(MapToResponse).ToList();
            return Ok(response);
        }

        // POST: /api/students
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<StudentResponse>> Create([FromBody] StudentRequest request)
        {
            var emailExists = await _studentService.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = $"A student with email '{request.Email}' already exists."
                });
            }

            var student = new Student
            {
                FullName = request.FullName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                EnrollmentDate = request.EnrollmentDate ?? DateTime.UtcNow
            };

            var created = await _studentService.AddStudentAsync(student);
            var response = MapToResponse(created);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT: /api/students/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] StudentRequest request)
        {
            var studentExists = await _studentService.StudentExistsAsync(id);
            if (!studentExists)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"Student with ID {id} was not found."
                });
            }

            var emailInUseByOther = await _studentService.EmailExistsAsync(request.Email, id);
            if (emailInUseByOther)
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = $"The email '{request.Email}' is already in use by another student."
                });
            }

            await _studentService.UpdateStudentAsync(id, request.FullName, request.Email, request.DateOfBirth);
            return NoContent();
        }

        // DELETE: /api/students/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentService.DeleteStudentAsync(id);
            if (!deleted)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"Student with ID {id} was not found."
                });
            }

            return NoContent();
        }

        // GET: /api/students/{id}/courses
        [HttpGet("{id:int}/courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourses(int id)
        {
            var courses = await _studentService.GetCoursesForStudentAsync(id);
            if (courses == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"Student with ID {id} was not found."
                });
            }

            return Ok(courses);
        }

        private static StudentResponse MapToResponse(Student student)
        {
            return new StudentResponse
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                EnrollmentDate = student.EnrollmentDate
            };
        }
    }
}
