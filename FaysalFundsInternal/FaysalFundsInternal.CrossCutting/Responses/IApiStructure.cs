namespace FaysalFundsInternal.CrossCutting.Responses
{
    internal interface IApiStructure
    {
        bool Status { get; set; }
        string Message { get; }
        int Code { get; set; }
        object? GetData();
    }
}
