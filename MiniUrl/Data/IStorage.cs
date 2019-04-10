using System;
namespace MiniUrl.Data
{
    public interface IStorage
    {
        bool AddValueToHash(string hashKey, string subKey, string value);
        bool UpdateValueToHash(string hashKey, string subKey, string value);
        string GetValueFromHash(string hashKey, string subKey);
        bool ExistInHash(string hashKey, string subKey);
    }
}
