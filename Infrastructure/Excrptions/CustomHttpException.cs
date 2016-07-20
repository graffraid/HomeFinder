namespace Infrastructure.Excrptions
{
    using System;
    using System.Net;

    public class CustomHttpException : Exception
    {
        public HttpStatusCode Code { get; }

        public override string Message { get; }

        public CustomHttpException(HttpStatusCode code, string message)
            : base(message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}