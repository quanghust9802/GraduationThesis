using Domain.Resource;
using System.Net;

namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        #region Fields
        public int ErrorCode { get; set; } = (int)HttpStatusCode.NotFound;
        #endregion

        #region Constructors
        public NotFoundException() : base(ResourceENG.Error_NotFound) { }

        public NotFoundException(int errorCode)
        {
            ErrorCode = errorCode;
        }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        #endregion
    }
}
