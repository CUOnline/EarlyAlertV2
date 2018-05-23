using RSS.Clients.Canvas.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;

namespace RSS.Clients.Canvas.Http
{
    public class RateLimit
    {
        public RateLimit() { }

        public RateLimit(IDictionary<string, string> responseHeaders)
        {
            Limit = (int)GetHeaderValueAsInt32Safe(responseHeaders, "X-RateLimit-Limit");
            Remaining = (int)GetHeaderValueAsInt32Safe(responseHeaders, "X-RateLimit-Remaining");
            ResetAsUtcEpochSeconds = GetHeaderValueAsInt32Safe(responseHeaders, "X-RateLimit-Reset");
        }

        public RateLimit(int limit, int remaining, long reset)
        {
            Limit = limit;
            Remaining = remaining;
            ResetAsUtcEpochSeconds = reset;
        }

        /// <summary>
        /// The maximum number of requests that the consumer is permitted to make per hour.
        /// </summary>
        public int Limit { get; private set; }

        /// <summary>
        /// The number of requests remaining in the current rate limit window.
        /// </summary>
        public int Remaining { get; private set; }

        /// <summary>
        /// The date and time at which the current rate limit window resets
        /// </summary>
        [Parameter(Key = "ignoreThisField")]
        public DateTimeOffset Reset { get { return ResetAsUtcEpochSeconds.FromUnixTime(); } }

        /// <summary>
        /// The date and time at which the current rate limit window resets - in UTC epoch seconds
        /// </summary>
        [Parameter(Key = "reset")]
        public long ResetAsUtcEpochSeconds { get; private set; }

        static long GetHeaderValueAsInt32Safe(IDictionary<string, string> responseHeaders, string key)
        {
            return !responseHeaders.TryGetValue(key, out string value) || value == null || !long.TryParse(value, out long result) ? 0 : result;
        }

        internal string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "Limit {0}, Remaining {1}, Reset {2} ", Limit, Remaining, Reset);
            }
        }

        /// <summary>
        /// Allows you to clone RateLimit
        /// </summary>
        /// <returns>A clone of <seealso cref="RateLimit"/></returns>
        public RateLimit Clone()
        {
            return new RateLimit
            {
                Limit = Limit,
                Remaining = Remaining,
                ResetAsUtcEpochSeconds = ResetAsUtcEpochSeconds
            };
        }
    }
}
