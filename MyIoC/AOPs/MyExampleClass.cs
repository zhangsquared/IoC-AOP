using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.AOPs
{
    public class MyExampleClass
    {
        /// <summary>
        /// use the key word virtual
        /// </summary>
        public virtual void MethodInterceptor()
        {
            Console.WriteLine("This is a interceptor");
        }

        /// <summary>
        /// no virtual, no interceptor
        /// </summary>
        public void MethodNoInterceptor()
        {
            Console.WriteLine("This is without interceptor");
        }
    }
}
