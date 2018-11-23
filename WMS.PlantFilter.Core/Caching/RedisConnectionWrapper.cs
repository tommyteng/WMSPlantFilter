using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace WMS.PlantFilter.Core.Caching
{
    public class RedisConnectionWrapper:IRedisConnectionWrapper
    {
        private readonly PFConfig _config;
        private readonly Lazy<string> _connectionString;

        private volatile ConnectionMultiplexer _connection;
        private volatile RedLockFactory _redisLockFactory;
        private readonly object _lock = new object();

        public RedisConnectionWrapper(PFConfig config) 
        {
            this._config = config;
            this._connectionString = new Lazy<string>(GetConnectionString);
            this._redisLockFactory = CreateRedisLockFactory();
        }

        protected RedLockFactory CreateRedisLockFactory()
        {
            var password = string.Empty;
            var useSsl = false;

            foreach (var option in GetConnectionString().Split(',').Where(op=>op.Contains('=')))
            {
                switch (option.Substring(0, option.IndexOf('=')).Trim().ToLowerInvariant())
                {
                    case "password":
                        password = option.Substring(option.IndexOf('=') + 1).Trim();
                        break;
                    case "ssl":
                        bool.TryParse(option.Substring(option.IndexOf('=') + 1).Trim(), out useSsl);
                        break;
                }
            }
            return new RedLockFactory(new RedLockConfiguration(
                GetEndPoints().Select(point => new RedLockEndPoint {
                    EndPoint = point,
                    Password = password,
                    Ssl = useSsl
                }).ToList()));
        }

        protected string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock) {
                if (_connection != null && _connection.IsConnected) return _connection;

                if (_connection != null)
                    _connection.Dispose();

                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();
            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1);
            }
        }

        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            using (var redisLock = _redisLockFactory.CreateLock(resource, expirationTime))
            {
                if (!redisLock.IsAcquired) return false;

                action();

                return true;
            }
        }

        public void Dispose()
        {
            if (_connection != null) _connection.Dispose();
            if (_redisLockFactory != null) _redisLockFactory.Dispose();

        }
    }
}
