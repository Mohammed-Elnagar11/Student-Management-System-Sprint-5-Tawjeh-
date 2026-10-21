using System;
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
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentsController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // POST: /api/enrollments
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest request)
        {
            var enrollment = new Enrollment
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                EnrollmentDate = request.EnrollmentDate ?? DateTime.UtcNow,
                Grade = request.Grade
            };

            var result = await _enrollmentService.AddEnrollmentAsync(enrollment);

            switch (result.Status)
            {
                case EnrollmentStatus.NotFound:
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Not Found",
                        Detail = result.Message
                    });

                case EnrollmentStatus.Duplicate:
                    return Conflict(new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflict",
                        Detail = result.Message
                    });

                case EnrollmentStatus.Success:
                default:
                    var response = new
                    {
                        result.Enrollment!.Id,
                        result.Enrollment.StudentId,
                        result.Enrollment.CourseId,
                        result.Enrollment.EnrollmentDate,
                        result.Enrollment.Grade
                    };
                    return StatusCode(StatusCodes.Status201Created, response);
            }
        }
    }
}
