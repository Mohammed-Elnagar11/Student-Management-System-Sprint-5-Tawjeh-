using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Student_Management_System__Sprint_5_Tawjeh_.Data;
using Student_Management_System__Sprint_5_Tawjeh_.Middleware;
using Student_Management_System__Sprint_5_Tawjeh_.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers & JSON configuration
builder.Services.AddControllers();

// ProblemDetails configuration for RFC 7807 standard error responses
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        // Ensure no stack trace or sensitive exception details leak into the JSON response
        context.ProblemDetails.Extensions.Remove("exception");
        context.ProblemDetails.Extensions.Remove("stackTrace");
        context.ProblemDetails.Extensions.Remove("trace");
    };
});

// DbContext registration with Scoped lifetime
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Server=DESKTOP-C6238S4\\SQLEXPRESS;Database=StudentSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;";
    options.UseSqlServer(connectionString);
});

// Dependency Injection: Register the 3 domain services with Scoped lifetime
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<EnrollmentService>();

var app = builder.Build();

// 1. Error Handling Middleware: handles unhandled exceptions and returns JSON 500 without stack trace
app.UseExceptionHandler();
app.UseStatusCodePages();

// 2. Custom Request Logging Middleware: logs HTTP method and path for every request
app.UseRequestLogging();

app.UseRouting();

// Endpoint for testing 500 ProblemDetails error handling without stack trace
app.MapGet("/api/test-error", () =>
{
    throw new InvalidOperationException("This is a simulated unexpected server error.");
});

app.MapControllers();

app.Run();
