using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainofResponsibility
{
    public class ZsquaredNonBlockingPipelineBuilder
    {
        private readonly IList<Func<ZsquaredDelegateAsync, ZsquaredDelegateAsync>> components
            = new List<Func<ZsquaredDelegateAsync, ZsquaredDelegateAsync>>();

        public ZsquaredNonBlockingPipelineBuilder Use(Func<ZsquaredDelegateAsync, ZsquaredDelegateAsync> middleware)
        {
            components.Add(middleware);
            return this;
        }

        public ZsquaredDelegateAsync Build()
        {
            ZsquaredDelegateAsync app = obj => Task.Run(() => obj = null); // final step
            foreach (var component in components.Reverse())
            {
                app = component(app);
            }
            return app;
        }
    }

    public static class MockNonBlockingBuilder
    {
        public static ZsquaredNonBlockingPipelineBuilder BuildNonBlockingMiddleware()
        {
            ZsquaredNonBlockingPipelineBuilder app = new ZsquaredNonBlockingPipelineBuilder();

            // config middleware
            app.Use(next =>
            {
                Console.WriteLine("middleware 1");
                ZsquaredDelegateAsync myDelegate = new ZsquaredDelegateAsync(async obj =>
                {
                    Console.WriteLine("middleware 1 start");
                    await next(obj);
                    Console.WriteLine("middleware 1 end");
                });
                return myDelegate;
            });
            app.Use(next =>
            {
                Console.WriteLine("middleware 2");
                ZsquaredDelegateAsync myDelegate = new ZsquaredDelegateAsync(async obj =>
                {
                    Console.WriteLine("middleware 2 start");
                    await next(obj);
                    Console.WriteLine("middleware 2 end");
                });
                return myDelegate;
            });
            app.Use(next =>
            {
                Console.WriteLine("middleware 3");
                ZsquaredDelegateAsync myDelegate = new ZsquaredDelegateAsync(async obj =>
                {
                    Console.WriteLine("middleware 3 start");
                    await next(obj);
                    Console.WriteLine("middleware 3 end");
                });
                return myDelegate;
            });

            return app;
        }
    }
}
