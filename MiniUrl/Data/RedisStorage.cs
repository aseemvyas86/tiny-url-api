using System;
using StackExchange.Redis;

namespace MiniUrl.Data
{
    public class RedisStorage:IStorage
    {
        private IDatabase _redisDatabase;
        private readonly IRedisConnector _redisConnector;

        public RedisStorage(IRedisConnector redisConnector)
        {
            _redisConnector = redisConnector;
            SelectDatabase(1);
        }

        public void SelectDatabase(int clientId)
        {
            _redisDatabase = _redisConnector.GetDatabase(clientId);
        }

        public bool ExistInHash(string hashKey, string subKey)
        {
            return _redisDatabase.HashExists(hashKey, subKey);
        }

        public bool AddValueToHash(string hashKey, string subKey, string value)
        {
            if (ExistInHash(hashKey, subKey))
            {
                throw new Exception("Key already exist in redis");
            }
            try
            {
                return _redisDatabase.HashSet(hashKey, subKey, value);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateValueToHash(string hashKey, string subKey, string value)
        {
            if (!ExistInHash(hashKey, subKey))
            {
                throw new InvalidOperationException("Invalid Operation");
            }
            try
            {
                string oldValue = _redisDatabase.HashGet(hashKey, subKey);

                if (!value.Equals(oldValue))
                {
                    return !_redisDatabase.HashSet(hashKey, subKey, value);
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddOrUpdateValueInHash(string hashTable, string subKey, string value)
        {

            try
            {
                return !_redisDatabase.HashSet(hashTable, subKey, value);
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public string GetValueFromHash(string hashKey, string subKey)
        {
            string result;
            try
            {
                result = _redisDatabase.HashGet(hashKey, subKey);

            }
            catch (Exception e)
            {
                result = default(string);
            }

            return result;
        }
    }
}
