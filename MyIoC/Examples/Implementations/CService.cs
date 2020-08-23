using MyIoC.Attributes;
using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class CService : ICService
    {
        private readonly IDALService mongo;

        public CService([MyShortname("Mongo")]IDALService dalService)
        {
            mongo = dalService;
        }

        [MyPropertyDI]
        [MyShortname("Mongo")]
        public IDALService AnotherMongo { get; set; }
    }
}
