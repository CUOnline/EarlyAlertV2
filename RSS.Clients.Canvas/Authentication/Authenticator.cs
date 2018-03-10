using RSS.Clients.Canvas.Helpers;
using RSS.Clients.Canvas.Http;
using RSS.Clients.Canvas.Interfaces.Authentication;
using RSS.Clients.Canvas.Interfaces.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RSS.Clients.Canvas.Authentication
{
    class Authenticator
    {
        readonly Dictionary<AuthenticationType, IAuthenticationHandler> authenticators =
            new Dictionary<AuthenticationType, IAuthenticationHandler>
            {
                { AuthenticationType.Oauth, new BearerAuthenticator() }
            };

        public Authenticator(ICredentialStore credentialStore)
        {
            Ensure.ArgumentNotNull(credentialStore, "credentialStore");
            CredentialStore = credentialStore;
        }

        public async Task Apply(IRequest request)
        {
            Ensure.ArgumentNotNull(request, "request");

            var credentials = await CredentialStore.GetCredentials().ConfigureAwait(false);
            authenticators[credentials.AuthenticationType].Authenticate(request, credentials);
        }

        public ICredentialStore CredentialStore { get; set; }
    }
}
