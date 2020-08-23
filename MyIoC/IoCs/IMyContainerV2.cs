using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.IoCs
{
    /// <summary>
    /// 6. life cycle support
    /// 
    /// ASP .NET Core has Kestrel
    /// When Kestrel started, create one Container
    /// Everytime when a new HTTP request coming in, clone the container or create a child container
    /// 
    /// AddTransient()
    /// AddSingleton() --- singleton across all requests
    /// AddScoped() --- basically singlton per request, singleton in child container
    /// AddPerThread()
    /// 
    /// 7. IoC + AOP
    /// 
    /// IOCContainerAOPExtension
    /// IOCInterceptor
    /// AbstractInterceptorAttribute
    /// </summary>
    public interface IMyContainerV2
    {
        IMyContainerV2 CreateChildContainer();

        void AddTransient<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface;

        void AddScoped<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface;

        void AddSingleton<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface;

        void AddPerThread<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface;

        TInterface Resolve<TInterface>(string shortName = null)
            where TInterface : class;
    }
}
