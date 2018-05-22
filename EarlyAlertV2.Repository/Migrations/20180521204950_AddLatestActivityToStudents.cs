using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EarlyAlertV2.Repository.Migrations
{
    public partial class AddLatestActivityToStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LatestActivity",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivityTimeMax",
                table: "ReportSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActivityTimeMin",
                table: "ReportSettings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestActivity",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ActivityTimeMax",
                table: "ReportSettings");

            migrationBuilder.DropColumn(
                name: "ActivityTimeMin",
                table: "ReportSettings");
        }
    }
}
