using System;

namespace ChainofResponsibility
{
    class Program
    {
        static void Main(string[] args)
        {
            ZsquaredPipelineBuilder app = MockBuilder.BuildMiddleware();
            ZsquaredDelegate zDelegate = app.Build();

            Console.WriteLine();

            ZsquaredContext context = new ZsquaredContext() { ID = 1, Name = "ZZ", IsSuccess = true };
            zDelegate.Invoke(context);
            Console.WriteLine(context.IsSuccess); // should return false

            Console.WriteLine();

            ZsquaredContext context2 = new ZsquaredContext() { ID = 2, Name = "Zsquared", IsSuccess = true };
            zDelegate.Invoke(context2);
            Console.WriteLine(context2.IsSuccess); // shoud return true

            Console.Read();
        }
    }
}
