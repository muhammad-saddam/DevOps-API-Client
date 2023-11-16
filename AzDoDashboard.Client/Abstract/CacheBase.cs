using AzDoDashboard.Client.Interfaces;

namespace AzDoDashboard.Client.Abstract
{
    public enum AccessType
    {
        /// <summary>
        /// data was fetched from cache
        /// </summary>
        Cached,
        /// <summary>
        /// data was fetched live
        /// </summary>
        Live
    }

    public abstract class CacheBase : ICache
    {
        public async Task<T> GetOrExecuteAsync<T>(string key, Func<Task<T>> accessor, TimeSpan timeToLive)
        {
            var data = await GetCacheDataAsync<T>(key);

            if (data.age > timeToLive || data.result is null)
            {
                var result = await accessor.Invoke();
                await SetCacheDataAsync(key, result);
                AccessType = AccessType.Live;
                return result;
            }

            AccessType = AccessType.Cached;
            return data.result;
        }

        public AccessType AccessType { get; private set; }

        protected abstract Task<(T result, TimeSpan age)> GetCacheDataAsync<T>(string key);

        /// <summary>
        /// implementation should mark current time in UTC
        /// </summary>
        protected abstract Task SetCacheDataAsync<T>(string key, T data);
    }
}
