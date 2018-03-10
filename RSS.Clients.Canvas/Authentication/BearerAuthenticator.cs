using RSS.Clients.Canvas.Helpers;
using RSS.Clients.Canvas.Http;
using RSS.Clients.Canvas.Interfaces.Authentication;
using RSS.Clients.Canvas.Interfaces.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RSS.Clients.Canvas.Authentication
{
    public class BearerAuthenticator : IAuthenticationHandler
    {
        public void Authenticate(IRequest request, Credentials credentials)
        {
            Ensure.ArgumentNotNull(request, "request");
            Ensure.ArgumentNotNull(credentials, "credentials");
            Ensure.ArgumentNotNull(credentials.Password, "credentials.Password");

            var token = credentials.GetToken();
            if (credentials.Login != null)
            {
                throw new InvalidOperationException("The Login is not null for a token authentication request. You " +
                    "probably did something wrong.");
            }
            if (token != null)
            {
                request.Headers["Authorization"] = string.Format(CultureInfo.InvariantCulture, "Bearer {0}", token);
            }
        }
    }
}
