using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class FService : IFService
    {
        public IEService E { get; }

        public FService(IEService e)
        {
            E = e;
        }
    }
}
