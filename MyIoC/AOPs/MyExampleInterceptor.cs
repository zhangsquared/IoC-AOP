using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.AOPs
{
    public class MyExampleInterceptor : StandardInterceptor
    {
        protected override void PreProceed(IInvocation invocation)
        {
            Console.WriteLine($"MyInterceptor pre proceed, method name: {invocation.Method.Name}");
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            Console.WriteLine($"MyInterceptor perform proceed, method name: {invocation.Method.Name}");
            base.PerformProceed(invocation);
        }

        protected override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine($"MyInterceptor post proceed, method name: {invocation.Method.Name}");
        }
    }

}
