using Arc4u.Caching;
using Arc4u.Configuration;
using Arc4u.Dependency.Attribute;
using Microsoft.Extensions.Options;
using System.IO;
using System.Runtime.Serialization.Json;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Infrastructure;

[Export(typeof(ISecureCache)), Shared]
class TokenCache : ISecureCache
{
    public TokenCache(IOptions<ApplicationConfig> config)
    {
        // Create a cache file from the application name in the config file + environment.
        var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "OAuth2");
        var info = Directory.CreateDirectory(path);

        _cacheFilePath = Path.Combine(path, string.Format("{0}_{1}_", config.Value.Environment.LoggingName, config.Value.Environment.Name));
    }

    private static readonly object FileLock = new object();
    private string _cacheFilePath;

    private bool disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
            if (disposing)
            {
                disposed = true;
            }
    }

    public TValue Get<TValue>(string key)
    {
        var path = ComputePath(key);

        if (!File.Exists(path)) return default(TValue);

        lock (FileLock)
        {
            var serializer = new DataContractJsonSerializer(typeof(TValue));

            serializer.ReadObject<TValue>(path, out var result);

            return result;
        }


    }

    private string ComputePath(string key)
    {
        return $"{_cacheFilePath}{Math.Abs(key.Trim().GetHashCode())}.json";
    }

    public void Initialize(string store)
    {
    }

    public bool Remove(string key)
    {
        try
        {
            lock (FileLock)
            {
                var path = ComputePath(key);

                File.Delete(path);

                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }

    }

    public bool TryGetValue<TValue>(string key, out TValue value)
    {
        try
        {
            value = Get<TValue>(key);
            return true;
        }
        catch (Exception)
        {
            value = default(TValue);
            return false;
        }
    }

    public void Put<T>(string key, T value)
    {
        var path = ComputePath(key);

        var serializer = new DataContractJsonSerializer(typeof(T));
        serializer.WriteObject(path, value);
    }

    public Task PutAsync<T>(string key, T value, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public void Put<T>(string key, TimeSpan timeout, T value, bool isSlided = false)
    {
        throw new NotImplementedException();
    }

    public Task PutAsync<T>(string key, TimeSpan timeout, T value, bool isSlided = false, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<TValue> GetAsync<TValue>(string key, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<TValue> TryGetValueAsync<TValue>(string key, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(string key, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}