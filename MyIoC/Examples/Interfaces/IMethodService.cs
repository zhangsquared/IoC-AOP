using MyIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Interfaces
{
    public interface IMethodService
    {
        [LogBefore]
        [LogAfter]
        [Monitor]
        void Show();

        void ShowWithoutLog();
    }
}
