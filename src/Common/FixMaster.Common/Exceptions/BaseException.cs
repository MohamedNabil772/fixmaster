namespace FixMaster.Common.Exceptions
{
    public abstract class BaseException : System.Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        protected BaseException(string message, string errorCode, int statusCode) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, "NOT_FOUND", 404) { }
    }

    public class ValidationException : BaseException
    {
        public ValidationException(string message) : base(message, "VALIDATION_ERROR", 400) { }
    }
}
