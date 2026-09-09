using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProbashiShield.Shared.MemCache
{
    public class MemCacheService : ICacheService
    {
        private readonly IMemoryCache _memCache;

        public MemCacheService(IMemoryCache memCache)
        {
            this._memCache = memCache;
        }

        public bool SetTextWithExpiration(string key, string value, double expirySeconds)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value) || expirySeconds <= 0)
            {
                return false;
            }

            var options = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(expirySeconds));
            _memCache.Set(key, value, options);

            return true;
        }

        public bool SetTextWithExpiration(string key, string value, DateTime tillTime)
        {
            var nowTime = DateTime.Now;
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value) || tillTime == DateTime.MinValue || (tillTime - nowTime).TotalSeconds <= 0)
            {
                return false;
            }

            var options = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds((tillTime - nowTime).TotalSeconds));
            _memCache.Set(key, value, options);

            return true;
        }

        public bool SetDataWithExpiration<T>(string key, T value, DateTime tillTime)
        {
            var nowTime = DateTime.Now;
            if (string.IsNullOrEmpty(key) || tillTime == DateTime.MinValue || (tillTime - nowTime).TotalSeconds <= 0)
            {
                return false;
            }

            var options = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds((tillTime - nowTime).TotalSeconds));
            _memCache.Set<T>(key, value, options);

            return true;
        }

        public bool SetListWithExpiration<T>(string key, List<T> value, double expirySeconds)
        {
            if (string.IsNullOrEmpty(key) || value == null || value.Count == 0 || expirySeconds <= 0)
            {
                return false;
            }

            var options = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(expirySeconds));

            _memCache.Set(key, value, options);

            return true;
        }

        public bool SetListWithExpiration<T>(string key, List<T> value, DateTime tillTime)
        {
            var nowTime = DateTime.Now;
            if (string.IsNullOrEmpty(key) || value == null || value.Count == 0 || tillTime == DateTime.MinValue || (tillTime - nowTime).TotalSeconds <= 0)
            {
                return false;
            }

            var options = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds((tillTime - nowTime).TotalSeconds));

            _memCache.Set(key, value, options);

            return true;
        }

        public string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            string text;

            if (!_memCache.TryGetValue(key, out text))
            {
                return null;
            }

            return text;
        }

        public List<T> GetList<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Enumerable.Empty<T>().ToList();
            }

            List<T> ListValue;

            if (!_memCache.TryGetValue<List<T>>(key, out ListValue))
            {
                return Enumerable.Empty<T>().ToList();
            }

            return ListValue;
        }

        public T GetData<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T value;
            if (!_memCache.TryGetValue<T>(key, out value))
            {
                return default(T);
            }

            return value;
        }

        public bool DeleteText(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            string text;

            if (!_memCache.TryGetValue(key, out text))
            {
                return true;
            }
            _memCache.Remove(key);
            return true;
        }

        public bool DeleteData<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            T value;
            if (!_memCache.TryGetValue<T>(key, out value))
            {
                return true;
            }

            _memCache.Remove(key);
            return true;
        }

        public bool DeleteList<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            List<T> ListValue;

            if (!_memCache.TryGetValue<List<T>>(key, out ListValue))
            {
                return true;
            }

            _memCache.Remove(key);
            return true;
        }

    }
}
