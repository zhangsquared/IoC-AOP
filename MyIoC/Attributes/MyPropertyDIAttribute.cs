using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // this attribute can only apply to properties
    public class MyPropertyDIAttribute : Attribute
    {
    }
}
