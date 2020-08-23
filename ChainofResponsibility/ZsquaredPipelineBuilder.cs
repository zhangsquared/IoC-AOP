using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChainofResponsibility
{
    public class ZsquaredPipelineBuilder
    {
        private readonly IList<Func<ZsquaredDelegate, ZsquaredDelegate>> components 
            = new List<Func<ZsquaredDelegate, ZsquaredDelegate>>();

        public ZsquaredPipelineBuilder Use(Func<ZsquaredDelegate, ZsquaredDelegate> middleware)
        {
            components.Add(middleware); // 1, 2, 3
            return this;
        }

        public ZsquaredDelegate Build()
        {
            ZsquaredDelegate zd = context =>
            {
                Console.WriteLine("final step");
                context.IsSuccess = false;
            };
            foreach (var component in components.Reverse()) // 3, 2, 1
            {
                zd = component(zd);
            }
            return zd; // 1
        }
    }

    public static class MockBuilder
    {
        public static ZsquaredPipelineBuilder BuildMiddleware()
        {
            ZsquaredPipelineBuilder app = new ZsquaredPipelineBuilder();

            // config middleware
            app.Use(next =>
            {
                Console.WriteLine("middleware 1 out");
                ZsquaredDelegate myDelegate = new ZsquaredDelegate(context =>
                {
                    Console.WriteLine("middleware 1 start");
                    next(context);
                    Console.WriteLine("middleware 1 end");
                });
                return myDelegate;
            });
            app.Use(next =>
            {
                Console.WriteLine("middleware 2 out");
                ZsquaredDelegate myDelegate = new ZsquaredDelegate(context =>
                {
                    Console.WriteLine("middleware 2 start");
                    next(context);
                    Console.WriteLine("middleware 2 end");
                });
                return myDelegate;
            });
            app.Use(next =>
            {
                Console.WriteLine("middleware 3 out");
                ZsquaredDelegate myDelegate = new ZsquaredDelegate(context =>
                {
                    Console.WriteLine("middleware 3 start");
                    if (context.Name.Equals("ZZ", StringComparison.InvariantCultureIgnoreCase)) next(context);
                    Console.WriteLine("middleware 3 end");
                });
                return myDelegate;
            });

            return app;
        }
    }
}
