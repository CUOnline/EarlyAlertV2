using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EarlyAlertV2.Repository.Migrations
{
    public partial class AddAssignmentGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentGroupId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AssignmentGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CanvasId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    GroupWeight = table.Column<double>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentGroups_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignmentGroupId",
                table: "Assignments",
                column: "AssignmentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentGroups_CourseId",
                table: "AssignmentGroups",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentGroups_AssignmentGroupId",
                table: "Assignments",
                column: "AssignmentGroupId",
                principalTable: "AssignmentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentGroups_AssignmentGroupId",
                table: "Assignments");

            migrationBuilder.DropTable(
                name: "AssignmentGroups");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignmentGroupId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentGroupId",
                table: "Assignments");
        }
    }
}
