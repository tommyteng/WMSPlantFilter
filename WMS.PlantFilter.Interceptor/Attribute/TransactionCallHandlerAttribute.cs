using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WMS.PlantFilter.Interceptor
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionCallHandlerAttribute : Attribute
    {
        /// <summary>  
        /// 超时时间  
        /// </summary>  
        public int Timeout { get; set; }

        /// <summary>  
        /// 事务范围  
        /// </summary>  
        public TransactionScopeOption ScopeOption { get; set; }

        /// <summary>  
        /// 事务隔离级别  
        /// </summary>  
        public IsolationLevel IsolationLevel { get; set; }
        public TransactionCallHandlerAttribute()
        {
            Timeout = 120;
            ScopeOption = TransactionScopeOption.Required;
            IsolationLevel = IsolationLevel.ReadCommitted;
        }
    }
}
