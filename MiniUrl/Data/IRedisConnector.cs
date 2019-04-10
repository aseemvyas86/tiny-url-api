using System;
using StackExchange.Redis;
namespace MiniUrl.Data
{
    public interface IRedisConnector
    {
        IDatabase GetDatabase(int databaseNumber = 1);
    }
}
