namespace FaysalFundsWrapper.Interfaces
{
    public interface IMainApiClient
    {
        Task<string> GetAsync(string endpoint);
        Task<TResponse> GetAsync<TResponse>(string endpoint);
        Task<string> PostAsync<TRequest>(string endpoint, TRequest data);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
    }
}
