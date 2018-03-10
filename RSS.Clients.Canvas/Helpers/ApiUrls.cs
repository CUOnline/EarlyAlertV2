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
        /// List Courses for a user
        /// </summary>
        public static Uri AllCourses(string userId)
        {
            return "users/{0}/courses".FormatUri(userId);
        }

        public static Uri User(string userId)
        {
            return "users/{0}".FormatUri(userId);
        }

        public static Uri Courses(string userId, bool includeGrading)
        {
            //https://canvas.instructure.com/doc/api/all_resources.html#method.courses.user_index
            return "users/{0}/courses".FormatUri(userId);
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
