﻿using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using RSS.Clients.Canvas.Helpers;
using RSS.Clients.Canvas.Http;
using RSS.Clients.Canvas.Interfaces.Http;
using RSS.Clients.Canvas.Models.Response;

namespace RSS.Clients.Canvas.Exceptions
{
    /// <summary>
    /// Represents errors that occur.
    /// </summary>
    [Serializable]
    public class ApiException : Exception
    {
        private static readonly IJsonSerializer _jsonSerializer = new SimpleJsonSerializer();

        /// <summary>
        /// Constructs an instance of ApiException
        /// </summary>
        public ApiException() : this(new Response())
        {
        }

        /// <summary>
        /// Constructs an instance of ApiException
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="httpStatusCode">The HTTP status code from the response</param>
        public ApiException(string message, HttpStatusCode httpStatusCode)
            : this(GetApiErrorFromExceptionMessage(message), httpStatusCode, null)
        {
        }

        /// <summary>
        /// Constructs an instance of ApiException
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception</param>
        public ApiException(string message, Exception innerException)
            : this(GetApiErrorFromExceptionMessage(message), 0, innerException)
        {
        }

        /// <summary>
        /// Constructs an instance of ApiException
        /// </summary>
        /// <param name="response">The HTTP payload from the server</param>
        public ApiException(IResponse response)
            : this(response, null)
        {
        }

        /// <summary>
        /// Constructs an instance of ApiException
        /// </summary>
        /// <param name="response">The HTTP payload from the server</param>
        /// <param name="innerException">The inner exception</param>
        public ApiException(IResponse response, Exception innerException)
            : base(null, innerException)
        {
            StatusCode = response.StatusCode;
            ApiError = GetApiErrorFromExceptionMessage(response);
            HttpResponse = response;
        }

        /// <summary>
        /// Constructs an instance of ApiException
        /// </summary>
        /// <param name="innerException">The inner exception</param>
        protected ApiException(ApiException innerException)
        {
            StatusCode = innerException.StatusCode;
            ApiError = innerException.ApiError;
        }

        protected ApiException(HttpStatusCode statusCode, Exception innerException)
            : base(null, innerException)
        {
            ApiError = new ApiError();
            StatusCode = statusCode;
        }

        protected ApiException(ApiError apiError, HttpStatusCode statusCode, Exception innerException)
            : base(null, innerException)
        {
            ApiError = apiError;
            StatusCode = statusCode;
        }

        public IResponse HttpResponse { get; private set; }

        public override string Message
        {
            get { return ApiErrorMessageSafe ?? "An error occurred with this API request"; }
        }

        /// <summary>
        /// The HTTP status code associated with the repsonse
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// The raw exception payload from the response
        /// </summary>
        public ApiError ApiError { get; private set; }

        static ApiError GetApiErrorFromExceptionMessage(IResponse response)
        {
            string responseBody = response != null ? response.Body as string : null;
            return GetApiErrorFromExceptionMessage(responseBody);
        }
        
        static ApiError GetApiErrorFromExceptionMessage(string responseContent)
        {
            try
            {
                if (!string.IsNullOrEmpty(responseContent))
                {
                    return _jsonSerializer.Deserialize<ApiError>(responseContent) ?? new ApiError(responseContent);
                }
            }
            catch (Exception)
            {
            }

            return new ApiError(responseContent);
        }
        
        /// <summary>
        /// Constructs an instance of ApiException.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected ApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null) return;
            StatusCode = (HttpStatusCode)info.GetInt32("HttpStatusCode");
            ApiError = (ApiError)info.GetValue("ApiError", typeof(ApiError));
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("HttpStatusCode", StatusCode);
            info.AddValue("ApiError", ApiError);
        }

        /// <summary>
        /// Get the inner error message from the API response
        /// </summary>
        /// <remarks>
        /// Returns null if ApiError is not populated
        /// </remarks>
        protected string ApiErrorMessageSafe
        {
            get
            {
                if (ApiError != null && !string.IsNullOrWhiteSpace(ApiError.Message))
                {
                    return ApiError.Message;
                }

                return null;
            }
        }

        /// <summary>
        /// Get the inner http response body from the API response
        /// </summary>
        /// <remarks>
        /// Returns empty string if HttpResponse is not populated or if
        /// response body is not a string
        /// </remarks>
        protected string HttpResponseBodySafe
        {
            get
            {
                return HttpResponse != null
                       && !HttpResponse.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
                       && HttpResponse.Body is string
                    ? (string)HttpResponse.Body : string.Empty;
            }
        }

        public override string ToString()
        {
            var original = base.ToString();
            return original + Environment.NewLine + HttpResponseBodySafe;
        }
    }
}
