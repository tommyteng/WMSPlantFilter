﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WMS.PlantFilter.Core.Caching
{
    /// <summary>
    /// 缓存扩展类
    /// </summary>
    public static class CacheExtensions
    {
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get<T>(cacheManager, key, 60, acquire);
        }

        public static T Get<T>(ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire.Invoke();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);

            return result;
        }

        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys) 
        {
            var regex = new Regex(pattern, RegexOptions.Singleline| RegexOptions.IgnoreCase| RegexOptions.Compiled);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())))
                cacheManager.Remove(key);
        }
    }
}
