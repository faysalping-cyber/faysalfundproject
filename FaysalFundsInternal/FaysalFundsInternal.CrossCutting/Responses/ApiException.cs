using FaysalFundsInternal.CrossCutting.Constants;

namespace FaysalFundsInternal.CrossCutting.Responses
{
    public class ApiException : Exception, IApiStructure
    {
        public bool Status { get; set; } = false;
        public string Message { get; }
        public int Code { get; set; } = ApiErrorCodes.Failed;
        public object? DataObject { get; set; }

        public object? GetData() => DataObject;

        public ApiException(
            string message,
            object? data = null,
            int? code = null) : base(message)
        {
            Message = message;
            DataObject = data;
            Code = code ?? ApiErrorCodes.Failed;
        }


    }

}
