using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCore8Day4.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: "Instructor")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Person", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Budget = table.Column<decimal>(type: "money", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InstructorID = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Department", x => x.DepartmentID);
                    table.ForeignKey(
                        name: "FK_dbo.Department_dbo.Instructor_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OfficeAssignment",
                columns: table => new
                {
                    InstructorID = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.OfficeAssignment", x => x.InstructorID);
                    table.ForeignKey(
                        name: "FK_dbo.OfficeAssignment_dbo.Instructor_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Person",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Course", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_dbo.Course_dbo.Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseInstructor",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    InstructorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.CourseInstructor", x => new { x.CourseID, x.InstructorID });
                    table.ForeignKey(
                        name: "FK_dbo.CourseInstructor_dbo.Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.CourseInstructor_dbo.Instructor_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Enrollment", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_dbo.Enrollment_dbo.Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Enrollment_dbo.Person_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentID",
                table: "Course",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseID",
                table: "CourseInstructor",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorID",
                table: "CourseInstructor",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorID",
                table: "Department",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseID",
                table: "Enrollment",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentID",
                table: "Enrollment",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorID",
                table: "OfficeAssignment",
                column: "InstructorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseInstructor");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "OfficeAssignment");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
