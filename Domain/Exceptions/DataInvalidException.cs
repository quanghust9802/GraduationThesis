using Domain.Resource;

namespace Domain.Exceptions
{
    public class DataInvalidException : Exception
    {
        #region Fields

        public int ErrorCode { get; set; } = 400;


        private readonly Dictionary<string, string>? _data;
        #endregion

        #region Constructors
        public DataInvalidException() : base(ResourceENG.Error_ValidateData) { }

        public DataInvalidException(int errorCode)
        {
            ErrorCode = errorCode;
        }
        public DataInvalidException(Dictionary<string, string> data)
        {
            _data = data;
        }

        public DataInvalidException(string message) : base(message) { }

        public DataInvalidException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
        public DataInvalidException(int errorCode, Dictionary<string, string> data)
        {
            ErrorCode = errorCode;
            _data = data;
        }

        public DataInvalidException(Dictionary<string, string> data, string message) : base(message)
        {
            _data = data;
        }

        #endregion

        public override Dictionary<string, string> Data => _data ?? new Dictionary<string, string> { };

    }
}
