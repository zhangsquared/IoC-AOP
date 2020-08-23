using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    public class LogAfterAttribute : AbstractInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Console.WriteLine("This is LogAfterAttribute 1");
                action.Invoke();
                Console.WriteLine($"This is LogAfterAttribute 2 at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss fff}");
            };
        }
    }
}
