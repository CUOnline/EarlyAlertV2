using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EarlyAlertV2.Repository.Migrations
{
    public partial class AddReportSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActivityWeight = table.Column<double>(nullable: false),
                    CommunicationWeight = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    GradeWeight = table.Column<double>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    LateAssignmentsWeight = table.Column<double>(nullable: false),
                    MissedAssignmentsWeight = table.Column<double>(nullable: false),
                    NumberOfActiveCoursesWeight = table.Column<double>(nullable: false),
                    PageViewsWeight = table.Column<double>(nullable: false),
                    ParticipationWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportSettings");
        }
    }
}
