using RSS.Clients.Canvas.Http;
using RSS.Clients.Canvas.Interfaces.Client;
using RSS.Clients.Canvas.Interfaces.Http;
using RSS.Clients.Canvas.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using RSS.Clients.Canvas.Clients;

namespace RSS.Clients.Canvas
{
    public class CanvasClient : ICanvasClient
    {
        public CanvasClient(Uri baseAddress, string oauthToken)
            : this(new Connection(baseAddress, new InMemoryCredentialStore(new Credentials(oauthToken, Authentication.AuthenticationType.Oauth))))
        {

        }

        public CanvasClient(IConnection connection)
        {
            Connection = Connection;
            var apiConnection = new ApiConnection(connection);

            CoursesClient = new CoursesClient(apiConnection);
            UsersClient = new UsersClient(apiConnection);
        }

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
                Connection.Credentials = value;
            }
        }

        public Uri BaseAddress
        {
            get
            {
                return Connection.BaseAddress;
            }
        }

        public IConnection Connection { get; private set; }

        public ICoursesClient CoursesClient { get; }

        public IUsersClient UsersClient { get; }
    }
}
