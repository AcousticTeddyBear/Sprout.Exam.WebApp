using System;
using System.Net;

namespace Sprout.Exam.Common.Exceptions
{
    public abstract class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(HttpStatusCode statusCode) => StatusCode = statusCode;

        public ApiException(string message, HttpStatusCode statusCode) : base(message) => StatusCode = statusCode;

        public ApiException(string message, Exception innerException, HttpStatusCode statusCode) : base(message, innerException) => StatusCode = statusCode;
    }
}
