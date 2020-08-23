using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Interfaces
{
    public interface IAService
    {
        IDALService DAL { get; }
    }
}
