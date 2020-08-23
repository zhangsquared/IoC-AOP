using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Interfaces
{
    public interface IDService
    {
        IDALService DAL { get; }
    }
}
