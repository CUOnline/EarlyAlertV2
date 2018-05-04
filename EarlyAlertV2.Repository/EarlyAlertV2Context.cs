using EarlyAlertV2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyAlertV2.Repository
{
    public class EarlyAlertV2Context : IdentityDbContext<ApplicationUser>
    {
        public EarlyAlertV2Context(DbContextOptions<EarlyAlertV2Context> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<StudentAssignmentSubmission> StudentAssignmentSubmissions { get; set; }
        public DbSet<AssignmentGroup> AssignmentGroups { get; set; }
        public DbSet<ReportSettings> ReportSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<StudentCourse>()
                .HasKey(x => new { x.StudentId, x.CourseId });

            builder.Entity<StudentCourse>()
                .HasOne(x => x.Student)
                .WithMany(x => x.StudentCourses)
                .HasForeignKey(x => x.StudentId);

            builder.Entity<StudentCourse>()
                .HasOne(x => x.Course)
                .WithMany(x => x.StudentCourses)
                .HasForeignKey(x => x.CourseId);

            builder.Entity<Assignment>()
                .HasOne(x => x.AssignmentGroup)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.AssignmentGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AssignmentGroup>()
                 .HasMany(x => x.Assignments)
                 .WithOne(x => x.AssignmentGroup)
                 .HasForeignKey(x => x.AssignmentGroupId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
