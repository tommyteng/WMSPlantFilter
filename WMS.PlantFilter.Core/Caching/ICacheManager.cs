using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.PlantFilter.Core.Caching
{
    public interface ICacheManager:IDisposable
    {
        T Get<T>(string key);

        void Set(string key, object obj, int cacheTime);

        /// <summary>
        /// 获取一个值，并指定是否缓存该值
        /// </summary>
        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        void Clear();
    }
}
