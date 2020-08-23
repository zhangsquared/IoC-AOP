using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class PropertyService : IPropertyService
    {
        public void Show()
        {
            Console.WriteLine("[x] Property Show");
        }
    }
}
