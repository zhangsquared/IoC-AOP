using Castle.DynamicProxy;
using MyIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyIoC.AOPs
{
    public class IOCInterceptor : StandardInterceptor
    {
        protected override void PreProceed(IInvocation invocation)
        {
            Console.WriteLine($"---------{invocation.Method.Name}---------");
            base.PreProceed(invocation);
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.Method;
            Action action = () => base.PerformProceed(invocation);
            var attributes = methodInfo.GetCustomAttributes<AbstractInterceptorAttribute>()
                .ToArray().Reverse();
            foreach (var attribute in attributes)
            {
                action = attribute.Do(invocation, action);
            }
            action.Invoke();
        }

        protected override void PostProceed(IInvocation invocation)
        {
            base.PostProceed(invocation);
        }
    }

}
