using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    public class LogBeforeAttribute : AbstractInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Console.WriteLine("This is LogBeforeAttribute 1");
                // log, validate parameters before calculation...
                Console.WriteLine($"This is LogBeforeAttribute 2 at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss fff}");
                action.Invoke();
            };
        }
    }
}
