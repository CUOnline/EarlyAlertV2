using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EarlyAlertV2.Repository.Migrations
{
    public partial class StudentSubmissionExcusedIsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Excused",
                table: "StudentAssignmentSubmissions",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Excused",
                table: "StudentAssignmentSubmissions",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
