using RSS.Clients.Canvas.Authentication;
using RSS.Clients.Canvas.Helpers;

namespace RSS.Clients.Canvas.Http
{
    public class Credentials
    {
        public Credentials(string token, AuthenticationType authType)
        {
            Login = null;
            Password = token;
            AuthenticationType = authType;
        }

        public string Login
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public AuthenticationType AuthenticationType
        {
            get;
            private set;
        }
    }
}