using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Interfaces
{
    public interface IWebService
    {
        IPropertyService PService { get; }

        IMethodService MService { get; }
    }
}
