namespace FaysalFundsInternal.Common
{
    public interface IAPICallService
    {
        Task<T> GetAsync<T>(string endpoint, Dictionary<string, string> headers = null);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, Dictionary<string, string> headers = null);
    }
}