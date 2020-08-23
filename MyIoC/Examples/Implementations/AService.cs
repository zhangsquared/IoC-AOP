using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class AService : IAService
    {
        public IDALService DAL { get; }

        public AService(IDALService dalService)
        {
            DAL = dalService;
        }
    }
}
