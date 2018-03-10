using System;
using System.Diagnostics;
using System.Globalization;

namespace RSS.Clients.Canvas.Models.Response
{
    [Serializable]
    public class ApiErrorDetail
    {
        public ApiErrorDetail() { }

        public ApiErrorDetail(string message, string code, string field, string resource)
        {
            Message = message;
            Code = code;
            Field = field;
            Resource = resource;
        }

        public string Message { get; protected set; }

        public string Code { get; protected set; }

        public string Field { get; protected set; }

        public string Resource { get; protected set; }

        internal string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "Message: {0}, Code: {1}, Field: {2}", Message, Code, Field);
            }
        }
    }
}