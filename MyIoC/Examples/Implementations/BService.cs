using MyIoC.Attributes;
using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class BService : IBService
    {
        public IAService A { get; }

        public IDALService DAL { get; }

        [MyChosenCtor]
        public BService(IDALService dalService)
        {
            DAL = dalService;
        }

        public BService(IDALService dalService, IAService aService)
        {
            DAL = dalService;
            A = aService;
        }

    }
}
