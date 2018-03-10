﻿using RSS.Clients.Canvas.Http;
using System.Threading.Tasks;

namespace RSS.Clients.Canvas.Interfaces.Http
{
    /// <summary>
    /// Abstraction for interacting with credentials
    /// </summary>
    public interface ICredentialStore
    {
        /// <summary>
        /// Retrieve the credentials from the underlying store
        /// </summary>
        /// <returns>A continuation containing credentials</returns>
        Task<Credentials> GetCredentials();
    }
}
