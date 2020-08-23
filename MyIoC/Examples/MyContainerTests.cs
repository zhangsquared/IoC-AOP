using MyIoC.Implementations;
using MyIoC.Interfaces;
using MyIoC.IoCs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyIoC.Examples
{
    public static class MyContainerTests
    {
        public static void TestV0()
        {
            IMyContainerV0 container = new MyContainerV0();
            container.Register<IAService, AService>();
            container.Register<IDALService, MySqlDALServce>();

            IAService a = container.Resolve<IAService>();
            IDALService dal = container.Resolve<IDALService>();
        }

        public static void TestV1()
        {
            IMyContainerV1 container = new MyContainerV1();

            // constructor DI
            container.Register<IWebService, WebServce>();
            container.Register<IAService, AService>();
            container.Register<IBService, BService>();
            container.Register<ICService, CService>();
            container.Register<IDService, DService>();
            container.Register<IDALService, MySqlDALServce>();
            // property DI
            container.Register<IPropertyService, PropertyService>();
            // method DI
            container.Register<IMethodService, MethodService>();
            IWebService w = container.Resolve<IWebService>();
            w.PService.Show();
            w.MService.Show();

            // constuctor with constant parameters
            container.Register<IIntService, IntService>(constParams: new object[] { 1, 3 });
            IIntService s = container.Resolve<IIntService>(); // s.GetI == 1; s.GetJ == 3

            // 1 interface, more than 1 implementation
            string shortName = "Mongo";
            container.Register<IDALService, MongoDALService>(shortName: shortName);
            IDALService mySql = container.Resolve<IDALService>();
            IDALService mongo = container.Resolve<IDALService>(shortName);
            ICService cService = container.Resolve<ICService>(); // here the IDAL for cService should be MongoDALService
            var another = cService.AnotherMongo;
        }

        public static void TestV2()
        {
            IMyContainerV2 container = new MyContainerV2();

            container.AddScoped<IAService, AService>();
            container.AddTransient<IDService, DService>();
            container.AddSingleton<IDALService, MySqlDALServce>();
            container.AddPerThread<IEService, EService>();
            container.AddPerThread<IFService, FService>();

            int loop = 100;

            // test singleton and transient
            for (int i = 0; i < loop; i++)
            {
                Task<IDService> dTask = Task.Run(() => container.Resolve<IDService>());
                Task<IDService> dTask2 = Task.Run(() => container.Resolve<IDService>());
                Task.WaitAll(dTask, dTask2); // blocking here
                IDService d = dTask.Result;
                IDService d2 = dTask2.Result;

                // test singleton
                bool same = ReferenceEquals(d.DAL, d2.DAL); // should be true
                if (!same) Console.WriteLine($"loop {i} -- error for DAL in d: d.DAL != d2.DAL");

                // test transient
                bool diff = ReferenceEquals(d, d2); // should be false
                if (diff) Console.WriteLine($"loop {i} -- error: d == d2");
            }

            // test scoped
            for (int i = 0; i < loop; i++)
            {
                Task<IAService[]> request = Task.Run(async () =>
                {
                    IMyContainerV2 childContainer = container.CreateChildContainer();
                    Task<IAService> aTask = Task.Run(() => childContainer.Resolve<IAService>());
                    Task<IAService> a2Task = Task.Run(() => childContainer.Resolve<IAService>());
                    return await Task.WhenAll(aTask, a2Task);
                });

                Task<IAService[]> request2 = Task.Run(async () =>
                {
                    IMyContainerV2 childContainer = container.CreateChildContainer();
                    Task<IAService> aTask = Task.Run(() => childContainer.Resolve<IAService>());
                    return await Task.WhenAll(aTask);
                });

                Task.WaitAll(request, request);
                bool sameA = ReferenceEquals(request.Result[0], request.Result[1]); // should be true
                if (!sameA) Console.WriteLine($"loop {i} -- error for A in the same request: a[0] != a[1]");

                bool diffA = ReferenceEquals(request.Result[0], request2.Result[0]); // should be false
                if (diffA) Console.WriteLine($"loop {i} -- error for A in different requests: a[0] == a2[0]");

                bool sameDal = ReferenceEquals(request.Result[0].DAL, request2.Result[0].DAL); // should be true
                if (!sameDal) Console.WriteLine($"loop {i} -- error for DAL in a: different DAL");
            }

            // test per thread
            for (int i = 0; i < 5; i++)
            {
                Task<IEService[]> t = Task.Run(() =>
                {
                    Console.WriteLine($"t thread: {Thread.CurrentThread.ManagedThreadId}");
                    IEService e = container.Resolve<IEService>();
                    IFService f = container.Resolve<IFService>();
                    Thread.Sleep(100);
                    return new IEService[] { e, f.E };
                });
                Task<IEService[]> t2 = Task.Run(() =>
                {
                    Console.WriteLine($"t2 thread: {Thread.CurrentThread.ManagedThreadId}");
                    IEService e = container.Resolve<IEService>();
                    return new IEService[] { e };
                });
                Task.WaitAll(t, t2);

                bool sameE = ReferenceEquals(t.Result[0], t.Result[1]); // should be true
                if (!sameE) Console.WriteLine($"error for E in the same thread: e[0] != e[1]");

                bool diffE = ReferenceEquals(t.Result[0], t2.Result[0]); // should be false
                if (diffE) Console.WriteLine($"error for A in different threads: e[0] == e2[0]");
            }
        }

        public static void TestAOP()
        {
            IMyContainerV2 container = new MyContainerV2();
            container.AddTransient<IMethodService, MethodService>();
            container.AddTransient<IPropertyService, PropertyService>();

            IMethodService m = container.Resolve<IMethodService>(); // EProxy
            IPropertyService p = container.Resolve<IPropertyService>(); // FProxy

            m.Show();
            Console.WriteLine();
            p.Show();

        }
    }
}
