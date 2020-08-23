using Castle.DynamicProxy;
using MyIoC.AOPs;
using MyIoC.Implementations;
using MyIoC.Interfaces;
using MyIoC.IoCs;
using System;

namespace MyIoC.Examples
{
    public static class AOPTests
    {
        public static void RunClass()
        {
            ProxyGenerator generator = new ProxyGenerator();
            MyExampleInterceptor interceptor = new MyExampleInterceptor();
            IOCInterceptor another = new IOCInterceptor();
            MyExampleClass test = generator.CreateClassProxy<MyExampleClass>(interceptor, another);

            Console.WriteLine($"the type of current class: {test.GetType()}, parent type: {test.GetType().BaseType}");
            test.MethodInterceptor();
            Console.WriteLine();
            test.MethodNoInterceptor();
        }

        public static void RunInterface()
        {
            ProxyGenerator generator = new ProxyGenerator();
            MyExampleInterceptor interceptor = new MyExampleInterceptor();

            IMyContainerV2 container = new MyContainerV2();
            container.AddTransient<IMethodService, MethodService>();
            IMethodService service = container.Resolve<IMethodService>();
            service.Show();

            Console.WriteLine();

            var proxy = generator.CreateInterfaceProxyWithTarget(service, interceptor);
            proxy.Show();
        }
    }
}
