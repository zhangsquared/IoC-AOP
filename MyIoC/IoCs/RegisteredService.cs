using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyIoC.IoCs
{
    public class RegisteredService
    {
        public Type TargetType { get; set; }

        public LifetimeEnum LifetimeType { get; set; }

        /// <summary>
        /// only set up for singleton
        /// </summary>
        public object SingletonInstance { get; set; }
        public Mutex SingletonMutex { get; } = new Mutex();
    }
}
