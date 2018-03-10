using RSS.Clients.Canvas.Http;
using RSS.Clients.Canvas.Interfaces.Client;
using RSS.Clients.Canvas.Interfaces.Http;
using RSS.Clients.Canvas.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas
{
    public class CanvasClient : ICanvasClient
    {
        public static readonly Uri CanvasApiUrl = new Uri("https://ucdenver.test.instructure.com/api/v1/");

        public void SetRequestTimeout(TimeSpan timeout)
        {
            Connection.SetRequestTimeout(timeout);
        }

        public ApiInfo GetLastApiInfo()
        {
            return Connection.GetLastApiInfo();
        }

        public Credentials Credentials
        {
            get
            {
                return Connection.Credentials;
            }
            set
            {
                Ensure.ArgumentNotNull(value, "value");
                Connection.Credentials = value;
            }
        }

        public Uri BaseAddress
        {
            get { return Connection.BaseAddress; }
        }

        public IConnection Connection { get; private set; }

        public ICoursesClient CoursesClient { get; }

        public IUsersClient UsersClient { get; }
    }
}
