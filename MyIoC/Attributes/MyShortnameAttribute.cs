using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class MyShortnameAttribute : Attribute
    {
        public string ShortName { get; }

        public MyShortnameAttribute(string shortName)
        {
            ShortName = shortName;
        }
    }
}
