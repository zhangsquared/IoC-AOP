using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class DService : IDService
    {
        public IDALService DAL { get; }

        public DService(IDALService dalService)
        {
            DAL = dalService;
        }
    }
}
