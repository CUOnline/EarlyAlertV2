using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EarlyAlertV2.Repository.Migrations
{
    public partial class UpdateAssignmentPointsPossibleNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PointsPossible",
                table: "Assignments",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PointsPossible",
                table: "Assignments",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
