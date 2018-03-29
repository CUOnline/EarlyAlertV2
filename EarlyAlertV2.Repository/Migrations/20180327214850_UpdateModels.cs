using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EarlyAlertV2.Repository.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "CanvasId",
                table: "Students",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SISUserId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CanvasId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueAt",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockAt",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PointsPossible",
                table: "Assignments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnlockAt",
                table: "Assignments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CanvasId = table.Column<int>(nullable: false),
                    CourseCode = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    EndAt = table.Column<DateTime>(nullable: true),
                    EnrollmentTermId = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    StartAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentAssignmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssignmentId = table.Column<int>(nullable: false),
                    CanvasAssignmentId = table.Column<int>(nullable: false),
                    CanvasId = table.Column<int>(nullable: false),
                    CanvasUserId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Excused = table.Column<bool>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    Late = table.Column<bool>(nullable: false),
                    Missing = table.Column<bool>(nullable: false),
                    Score = table.Column<double>(nullable: true),
                    StudentId = table.Column<int>(nullable: false),
                    WorkflowState = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAssignmentSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAssignmentSubmissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAssignmentSubmissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourse",
                columns: table => new
                {
                    StudentId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourse", x => new { x.StudentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_StudentCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourse_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignmentSubmissions_AssignmentId",
                table: "StudentAssignmentSubmissions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignmentSubmissions_StudentId",
                table: "StudentAssignmentSubmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourse_CourseId",
                table: "StudentCourse",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropTable(
                name: "StudentAssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "StudentCourse");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CanvasId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SISUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CanvasId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "DueAt",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "LockAt",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "PointsPossible",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "UnlockAt",
                table: "Assignments");

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                });
        }
    }
}
