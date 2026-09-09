using System;
using System.Collections.Generic;

namespace ProbashiShield.Shared.MemCache
{
    public interface ICacheService
    {
        bool SetTextWithExpiration(string key, string value, double expirySeconds);
        bool SetTextWithExpiration(string key, string value, DateTime tillTime);
        bool SetDataWithExpiration<T>(string key, T value, DateTime tillTime);
        bool SetListWithExpiration<T>(string key, List<T> value, double expirySeconds);
        bool SetListWithExpiration<T>(string key, List<T> value, DateTime tillTime);
        string GetText(string key);
        T GetData<T>(string key);
        List<T> GetList<T>(string key);
        bool DeleteText(string key);
        bool DeleteData<T>(string key);
        bool DeleteList<T>(string key);
    }
}
