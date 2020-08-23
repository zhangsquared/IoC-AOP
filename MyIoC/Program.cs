using MyIoC.Examples;
using MyIoC.Implementations;
using MyIoC.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyIoC
{
    class Program
    {
        static void Main(string[] args)
        {
            MyContainerTests.TestV0();
            MyContainerTests.TestV1();
            MyContainerTests.TestV2();

            AOPTests.RunClass();
            AOPTests.RunInterface();

            MyContainerTests.TestAOP();

            Console.ReadKey();
        }

        
    }
}
