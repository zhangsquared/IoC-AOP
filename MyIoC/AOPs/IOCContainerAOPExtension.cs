using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.AOPs
{
    public static class IOCContainerAOPExtension
    {
        public static T AOP<T>(this T t) where T : class
        {
            ProxyGenerator generator = new ProxyGenerator();
            IOCInterceptor interceptor = new IOCInterceptor();
            T proxy = generator.CreateInterfaceProxyWithTarget(t, interceptor);
            return proxy;
        }
    }
}
