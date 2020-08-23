using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MyIoC.Attributes
{
    public class MonitorAttribute : AbstractInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Console.WriteLine("This is MonitorAttribute 1");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                action.Invoke();
                stopwatch.Stop();
                Console.WriteLine($"This is MonitorAttribute 2. This action takes {stopwatch.ElapsedMilliseconds} ms");
            };
            

        }
    }
}
