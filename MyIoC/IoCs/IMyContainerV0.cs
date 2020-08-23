using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.IoCs
{
    /// <summary>
    /// 0. create instance, with 1 layer of constuctor DI
    /// </summary>
    public interface IMyContainerV0
    {
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;

        TInterface Resolve<TInterface>();
    }
}
