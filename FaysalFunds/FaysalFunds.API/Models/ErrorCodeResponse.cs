namespace FaysalFunds.API.Models
{
    public class ErrorCodeResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorId { get; set; }
        public ErrorCodeResponse(int errCode, string errorMsg,string errorId)
        {
            ErrorCode = errCode;
            ErrorMessage = errorMsg;
            ErrorId = errorId;
        }
    }
}
