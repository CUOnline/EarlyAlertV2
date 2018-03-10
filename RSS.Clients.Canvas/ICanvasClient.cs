using RSS.Clients.Canvas.Interfaces.Client;
using RSS.Clients.Canvas.Interfaces.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas
{
    public interface ICanvasClient : IApiInfoProvider
    {
        /// <summary>
        /// Set the GitHub Api request timeout.
        /// Useful to set a specific timeout for lengthy operations, such as uploading release assets
        /// </summary>
        /// <remarks>
        /// See more information here: https://technet.microsoft.com/library/system.net.http.httpclient.timeout(v=vs.110).aspx
        /// </remarks>
        /// <param name="timeout">The Timeout value</param>
        void SetRequestTimeout(TimeSpan timeout);

        /// <summary>
        /// Provides a client connection to make rest requests to HTTP endpoints.
        /// </summary>
        IConnection Connection { get; }

        ICoursesClient CoursesClient { get; }

        IUsersClient UsersClient { get; }
    }
}
