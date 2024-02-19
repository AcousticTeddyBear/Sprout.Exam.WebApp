using System;
using System.Net;

namespace Sprout.Exam.Common.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound) { }

        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException, HttpStatusCode.NotFound) { }
    }
}
