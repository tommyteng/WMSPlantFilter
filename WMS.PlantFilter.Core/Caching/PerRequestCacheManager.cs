using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WMS.PlantFilter.Core.Caching
{
    public partial class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }

        protected virtual IDictionary GetItems()
        {
            if (this._context != null)
                return this._context.Items;
            return null;
        }

        public virtual T Get<T>(string key)
        {
            var items = this.GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];

        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = this.GetItems();
            if (items == null) return;

            if (data != null) {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        public virtual bool IsSet(string key)
        {
            var items = this.GetItems();
            if (items == null) return false;

            return items[key] != null;
        }

        public virtual void Remove(string key)
        {
            var items = this.GetItems();
            if (items == null) return;

            items.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var items = this.GetItems();
            if (items == null) return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        public virtual void Clear()
        {
            var items = this.GetItems();
            if (items == null) return;

            items.Clear();
        }

        public virtual void Dispose()
        {
            
        }
    }
}
