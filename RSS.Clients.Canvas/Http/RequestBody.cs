namespace RSS.Clients.Canvas.Http
{
    /// <summary>
    /// Container for the static <see cref="Empty"/> method that represents an
    /// intentional empty request body to avoid overloading <code>null</code>.
    /// </summary>
    public static class RequestBody
    {
        public static object Empty = new object();
    }
}