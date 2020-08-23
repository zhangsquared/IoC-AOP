using MyIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Interfaces
{
    public interface IPropertyService
    {
        [LogBefore]
        [LogAfter]
        [Monitor]
        void Show();
    }
}
