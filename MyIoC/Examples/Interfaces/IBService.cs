using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Interfaces
{
    public interface IBService
    {
        IDALService DAL { get; }

        IAService A { get; }
    }
}
