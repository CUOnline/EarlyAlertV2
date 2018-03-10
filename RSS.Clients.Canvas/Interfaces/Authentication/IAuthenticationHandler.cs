using RSS.Clients.Canvas.Http;
using RSS.Clients.Canvas.Interfaces.Http;

namespace RSS.Clients.Canvas.Interfaces.Authentication
{
    interface IAuthenticationHandler
    {
        void Authenticate(IRequest request, Credentials credentials);
    }
}