using MyIoC.Attributes;
using MyIoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.Implementations
{
    public class WebServce : IWebService
    {
        private readonly IAService a;
        private readonly IBService b;       

        public WebServce(IAService aService, IBService bService)
        {
            a = aService;
            b = bService;
        }

        [MyPropertyDI]
        public IPropertyService PService { get; set; }

        public IMethodService MService { get; private set; }

        [MyMethodDI]
        public void InitA2(IMethodService mService)
        {
            MService = mService;
        }
    }
}
