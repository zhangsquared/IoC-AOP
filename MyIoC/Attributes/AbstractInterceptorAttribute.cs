using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    /// <summary>
    /// need to put on interface method!
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class AbstractInterceptorAttribute : Attribute
    {
        public abstract Action Do(IInvocation invocation, Action action);
    }
}
