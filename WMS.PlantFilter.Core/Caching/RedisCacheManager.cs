using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
namespace WMS.PlantFilter.Core.Caching
{
    public partial class RedisCacheManager : ICacheManager
    {
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;
        private readonly ICacheManager _perRequestCacheManager;

        public RedisCacheManager(IRedisConnectionWrapper connectionWrapper, ICacheManager perRequestCacheManager) 
        {
            this._perRequestCacheManager = perRequestCacheManager;
            this._connectionWrapper = connectionWrapper;
            this._db = this._connectionWrapper.GetDatabase();
        }
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        protected virtual byte[] Serialize(object data)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        public virtual T Get<T>(string key)
        {
            if (this._perRequestCacheManager != null && this._perRequestCacheManager.IsSet(key))
                return this._perRequestCacheManager.Get<T>(key);

            var rValue = _db.StringGet(key);
            if (!rValue.HasValue)
                return default(T);
            var result = Deserialize<T>(rValue);

            this._perRequestCacheManager.Set(key, result, 0);
            return result;
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null) return;

            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }

        

        public virtual bool IsSet(string key)
        {
            if (this._perRequestCacheManager.IsSet(key))
                return true;

            return this._db.KeyExists(key);
        }

        public virtual void Remove(string key)
        {
            this._db.KeyDelete(key);
            this._perRequestCacheManager.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            foreach (var ep in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(ep);
                var keys = server.Keys(database: _db.Database, pattern: "*" + pattern + "*");
                foreach (var key in keys)
                    Remove(key);
            }
        }

        public virtual void Clear()
        {
            foreach (var ep in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(ep);

                var keys = server.Keys(this._db.Database);
                foreach (var key in keys)
                {
                    Remove(key);
                }
            }
        }

        public virtual void Dispose()
        {
            if (this._connectionWrapper != null)
                this._connectionWrapper.Dispose();
        }
    }
}
