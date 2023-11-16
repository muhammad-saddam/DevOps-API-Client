using AzDoDashboard.Client.Abstract;
using System.Text.Json;

namespace AzDoDashboard.CLI.Services
{
    internal class LocalCache : CacheBase
    {
        private readonly string _basePath;

        public LocalCache(string folderName)
        {
            _basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folderName);
        }

        protected override async Task<(T result, TimeSpan age)> GetCacheDataAsync<T>(string key)
        {
            var fileName = GetFilename(key);
            if (File.Exists(fileName))
            {
                var json = await File.ReadAllTextAsync(fileName);
                var data = JsonSerializer.Deserialize<CachedData<T>>(json);
                return (data.Data, DateTime.UtcNow.Subtract(data.Timestamp));
            }

            return await Task.FromResult((default(T), TimeSpan.MaxValue));
        }

        protected override async Task SetCacheDataAsync<T>(string key, T data)
        {
            var store = new CachedData<T>(data)
            {
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(store);
            var fileName = GetFilename(key);
            await File.WriteAllTextAsync(fileName, json);
        }

        private string GetFilename(string key)
        {
            var result = Path.Combine(_basePath, key + ".json");
            var folder = Path.GetDirectoryName(result);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            return result;
        }

        private class CachedData<T>
        {
            public CachedData()
            {
            }

            public CachedData(T data)
            {
                Data = data;
            }

            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
            public T Data { get; set; }
        }
    }
}
