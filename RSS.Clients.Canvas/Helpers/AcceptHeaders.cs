using System.Diagnostics.CodeAnalysis;

namespace RSS.Clients.Canvas.Helpers
{
    public static class AcceptHeaders
    {
        public const string StableVersionHtml = "text/html";

        public const string StableVersionJson = "application/json; charset=utf-8";
        
        /// <summary>
        /// Combines multiple preview headers.
        /// </summary>
        /// <param name="headers">Accept header values that will be combine to single Accept header.</param>
        /// <returns>Accept header value.</returns>
        public static string Concat(params string[] headers)
        {
            return string.Join(",", headers);
        }
    }
}
