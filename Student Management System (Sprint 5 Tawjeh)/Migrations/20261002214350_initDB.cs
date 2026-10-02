using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Student_Management_System__Sprint_5_Tawjeh_.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "FullName" },
                values: new object[,]
                {
                    { 1, "Dr. Sarah Johnson" },
                    { 2, "Prof. Ahmed Hassan" },
                    { 3, "Dr. Emily Chen" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfBirth", "Email", "EnrollmentDate", "FullName" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "ali.mohamed@university.edu", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali Mohamed" },
                    { 2, new DateTime(2001, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara.ahmed@university.edu", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sara Ahmed" },
                    { 3, new DateTime(1999, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "omar.khaled@university.edu", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar Khaled" },
                    { 4, new DateTime(2002, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "nour.hassan@university.edu", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nour Hassan" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Credits", "Description", "InstructorId", "Title" },
                values: new object[,]
                {
                    { 1, 3, "Fundamental concepts of programming and computational thinking.", 1, "Introduction to Computer Science" },
                    { 2, 4, "Relational database design, SQL, and modern ORM frameworks.", 2, "Database Systems" },
                    { 3, 4, "Core data structures: lists, trees, graphs, and algorithm analysis.", 1, "Data Structures and Algorithms" },
                    { 4, 3, "Frontend and backend web development with modern frameworks.", 3, "Web Development" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "Id", "CourseId", "EnrollmentDate", "Grade", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 88, 1 },
                    { 2, 2, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 92, 1 },
                    { 3, 3, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 4, 1, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 76, 2 },
                    { 5, 4, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 85, 2 },
                    { 6, 2, new DateTime(2021, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 95, 3 },
                    { 7, 3, new DateTime(2021, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 78, 3 },
                    { 8, 4, new DateTime(2022, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, 3 },
                    { 9, 1, new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4 },
                    { 10, 2, new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorId",
                table: "Courses",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_CourseId",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Instructors");
        }
    }
}
