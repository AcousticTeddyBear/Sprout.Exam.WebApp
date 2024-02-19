using System;
using System.Net;

namespace Sprout.Exam.Common.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException() : base(HttpStatusCode.BadRequest) { }

        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest) { }

        public BadRequestException(string message, Exception innerException) : base(message, innerException, HttpStatusCode.BadRequest) { }
    }
}
