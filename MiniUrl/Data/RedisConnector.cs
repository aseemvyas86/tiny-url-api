using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace MiniUrl.Data
{
    public class RedisConnector:IRedisConnector
    {
        private readonly string _configurationString;
        private IConfiguration _configuration;
        private static ConnectionMultiplexer _redis;

        public RedisConnector(IConfiguration configuration)
        {
            _configuration = configuration;
            _configurationString = _configuration["RedisCache"];
            Initialize();
        }

        private void Initialize()
        {
            if (_redis != null && _redis.IsConnected)
            {
                return;
            }

            if (string.IsNullOrEmpty(_configurationString))
            {
                const string message = "Invalid cache server configuration";
                throw new ArgumentException(message);
            }

            if (_redis == null)
            {
                Connect();
            }
        }

        private bool Connect()
        {
            if (_redis != null && _redis.IsConnected) return true;

            try
            {
                ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(_configurationString);
                configurationOptions.AllowAdmin = true;
                _redis = ConnectionMultiplexer.Connect(configurationOptions);
            }
            catch (Exception ex)
            {
                throw new Exception(" it is not possible to redis server");
            }

            return _redis.IsConnected;
        }


        public IDatabase GetDatabase(int databaseNumber = 0)
        {
            if (Connect())
            {
                return databaseNumber > 0
                    ? _redis.GetDatabase(databaseNumber)
                    : _redis.GetDatabase(0);
            }

            return null;
        }

        public void Dispose()
        {
            _redis.Close();
            _redis.Dispose();
            _redis = null;
        }
    }
}
