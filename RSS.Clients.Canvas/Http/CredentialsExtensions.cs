using RSS.Clients.Canvas.Helpers;

namespace RSS.Clients.Canvas.Http
{
    public static class CredentialsExtensions
    {
        public static string GetToken(this Credentials credentials)
        {
            Ensure.ArgumentNotNull(credentials, "credentials");

            return credentials.Password;
        }
    }
}