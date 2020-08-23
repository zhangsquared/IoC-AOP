using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MyMethodDIAttribute : Attribute
    {
    }
}
