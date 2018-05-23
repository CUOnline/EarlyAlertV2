using RSS.Clients.Canvas.Helpers;
using RSS.Clients.Canvas.Http;
using System.Threading.Tasks;

namespace RSS.Clients.Canvas.Interfaces.Http
{
    /// <summary>
    /// Abstraction for interacting with credentials
    /// </summary>
    public class InMemoryCredentialStore : ICredentialStore
    {
        readonly Credentials _credentials;

        /// <summary>
        /// Create an instance of the InMemoryCredentialStore
        /// </summary>
        /// <param name="credentials"></param>
        public InMemoryCredentialStore(Credentials credentials)
        {
            _credentials = credentials;
        }

        /// <summary>
        /// Retrieve the credentials from the underlying store
        /// </summary>
        /// <returns>A continuation containing credentials</returns>
        public Task<Credentials> GetCredentials()
        {
            return Task.FromResult(_credentials);
        }
    }
}