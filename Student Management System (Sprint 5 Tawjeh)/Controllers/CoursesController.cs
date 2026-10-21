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
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: /api/courses/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"Course with ID {id} was not found."
                });
            }

            return Ok(new
            {
                course.Id,
                course.Title,
                course.Credits,
                course.Description
            });
        }

        // POST: /api/courses
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
        {
            var course = new Course
            {
                Title = request.Title,
                Credits = request.Credits,
                Description = request.Description ?? string.Empty
            };

            var created = await _courseService.AddCourseAsync(course);

            var response = new
            {
                created.Id,
                created.Title,
                created.Credits,
                created.Description
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        // DELETE: /api/courses/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            switch (result)
            {
                case CourseDeleteResult.NotFound:
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Not Found",
                        Detail = $"Course with ID {id} was not found."
                    });

                case CourseDeleteResult.HasEnrollments:
                    return Conflict(new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflict",
                        Detail = $"Cannot delete course with ID {id} because it has active enrollments."
                    });

                case CourseDeleteResult.Success:
                default:
                    return NoContent();
            }
        }

        // GET: /api/courses/{id}/students
        [HttpGet("{id:int}/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudents(int id)
        {
            var students = await _courseService.GetStudentsInCourseAsync(id);
            if (students == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"Course with ID {id} was not found."
                });
            }

            return Ok(students);
        }

        // GET: /api/courses/{id}/average-grade
        [HttpGet("{id:int}/average-grade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAverageGrade(int id)
        {
            var (exists, averageGrade) = await _courseService.GetAverageGradeForCourseAsync(id);
            if (!exists)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = $"Course with ID {id} was not found."
                });
            }

            return Ok(new
            {
                courseId = id,
                averageGrade = averageGrade
            });
        }
    }
}
