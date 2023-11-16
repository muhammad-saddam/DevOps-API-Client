
using AzDoDashboard.Client.Abstract;

namespace AzDoDashboard.Client.Interfaces
{
    public interface ICache
    {
        AccessType AccessType { get; }

        Task<T> GetOrExecuteAsync<T>(string key, Func<Task<T>> accessor, TimeSpan timeToLive);
    }
}