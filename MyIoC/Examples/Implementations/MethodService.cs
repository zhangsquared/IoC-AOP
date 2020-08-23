using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class MethodService : IMethodService
    {
        public void Show()
        {
            Console.WriteLine("[x] MethodService Show");
        }

        public void ShowWithoutLog()
        {
            Console.WriteLine("[x] MethodService ShowWithoutLog");
        }
    }
}
