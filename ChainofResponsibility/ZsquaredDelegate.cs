using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChainofResponsibility
{
    public delegate void ZsquaredDelegate(ZsquaredContext z);

    public delegate Task ZsquaredDelegateAsync(ZsquaredContext z);
}
