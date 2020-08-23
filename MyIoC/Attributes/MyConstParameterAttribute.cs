using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class MyConstParamAttribute : Attribute
    {
    }
}
