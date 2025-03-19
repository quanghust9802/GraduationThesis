using Domain.Resource;
using System.Net;

namespace Domain.Exceptions
{
    public class ForbiddenException : Exception
    {

        #region Fields
        public int ErrorCode { get; set; } = (int)HttpStatusCode.Forbidden;

        #endregion


        #region Constructors
        public ForbiddenException() : base(ResourceENG.Error_Conflict) { }

        public ForbiddenException(int errorCode)
        {
            ErrorCode = errorCode;
        }

        public ForbiddenException(string message) : base(message) { }

        public ForbiddenException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        #endregion
    }
}