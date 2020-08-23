using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor)] // this attribute can only apply to constructors
    public class MyChosenCtorAttribute : Attribute
    {
    }
}
