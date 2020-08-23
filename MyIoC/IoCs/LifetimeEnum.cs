using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.IoCs
{
    public enum LifetimeEnum
    {
        Transient,
        Scope,
        Singleton,
        PerThread
    }
}
