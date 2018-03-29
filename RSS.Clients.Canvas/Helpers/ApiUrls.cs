using System;
using System.Diagnostics.CodeAnalysis;

namespace RSS.Clients.Canvas.Helpers
{
    public static class ApiUrls
    {
        public static Uri Course(string courseId)
        {
            return "courses/{0}".FormatUri(courseId);
        }

        public static Uri AllCourses()
        {
            return "courses".FormatUri();
        }

        /// <summary>
        /// List assignments in course.
        /// </summary>
        public static Uri CourseAssignments(int courseId)
        {
            return "courses/{0}/assignments".FormatUri(courseId);
        }

        /// <summary>
        /// List Courses for a user
        /// </summary>
        public static Uri UserCourses(int userId)
        {
            return "users/{0}/courses".FormatUri(userId);
        }

        public static Uri User(int id)
        {
            return "users/{0}".FormatUri(id);
        }

        public static Uri User(string sisUserId)
        {
            return "users/sis_user_id:{0}".FormatUri(sisUserId);
        }
        
        public static Uri Assignment(int assignmentId)
        {
            return "assignments/{0}".FormatUri(assignmentId);
        }

        public static Uri UserSubmissions(int courseId)
        {
            return "courses/{0}/students/submissions".FormatUri(courseId);
        }

        public static Uri AssignmentGroups(int courseId)
        {
            return "courses/{0}/assignment_groups".FormatUri(courseId);
        }



        /// <summary>
        /// Returns the <see cref="Uri"/> that returns all public repositories in
        /// response to a GET request.
        /// </summary>
        public static Uri AllPublicRepositories()
        {
            return "repositories".FormatUri();
        }


        /// <summary>
        /// Returns the <see cref="Uri"/> that returns all public repositories in
        /// response to a GET request.
        /// </summary>
        /// <param name="since">The integer Id of the last Repository that you’ve seen.</param>
        public static Uri AllPublicRepositories(long since)
        {
            return "repositories?since={0}".FormatUri(since);
        }


    }
}
