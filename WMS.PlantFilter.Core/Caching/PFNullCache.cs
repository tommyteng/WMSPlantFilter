using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.PlantFilter.Core.Caching
{
    public partial class PFNullCache : ICacheManager
    {
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        public virtual void Set(string key, object obj, int cacheTime)
        {
            
        }

        public virtual bool IsSet(string key)
        {
            return false;
        }

        public virtual void Remove(string key)
        {
             
        }

        public virtual void RemoveByPattern(string pattern)
        {
            
        }

        public virtual void Clear()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
