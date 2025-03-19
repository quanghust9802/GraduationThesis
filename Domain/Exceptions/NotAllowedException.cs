using Domain.Resource;
using System.Net;

namespace Domain.Exceptions
{
    public class NotAllowedException : Exception
    {

        #region Fields
        public int ErrorCode { get; set; } = (int)HttpStatusCode.MethodNotAllowed;

        #endregion


        #region Constructors
        public NotAllowedException() : base(ResourceENG.Error_Conflict) { }

        public NotAllowedException(int errorCode)
        {
            ErrorCode = errorCode;
        }

        public NotAllowedException(string message) : base(message) { }

        public NotAllowedException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        #endregion
    }
}