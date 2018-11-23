using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WMS.PlantFilter.Interceptor
{
    // 事务 拦截器   （oracel下有错误）
    /// </summary>  
    public class TransactionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            TransactionCallHandlerAttribute transaction =
                methodInfo.GetCustomAttributes<TransactionCallHandlerAttribute>(true).FirstOrDefault();
            if (transaction != null)
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = transaction.IsolationLevel;
                transactionOptions.Timeout = new TimeSpan(0, 0, transaction.Timeout);
                using (TransactionScope scope = new TransactionScope(transaction.ScopeOption, transactionOptions))
                {
                    try
                    {
                        invocation.Proceed();
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
