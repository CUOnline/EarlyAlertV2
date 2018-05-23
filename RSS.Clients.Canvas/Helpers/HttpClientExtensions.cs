using System.Threading;
using System.Threading.Tasks;
using RSS.Clients.Canvas.Interfaces.Http;

namespace RSS.Clients.Canvas.Helpers
{
    public static class HttpClientExtensions
    {
        public static Task<IResponse> Send(this IHttpClient httpClient, IRequest request)
        {
            return httpClient.Send(request, CancellationToken.None);
        }
    }
}