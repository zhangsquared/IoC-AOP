using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CircularReference.Services
{
    public class ChickenService : IChickenService
    {
        private readonly IEggService s;

        public ChickenService(IEggService bService)
        {
            s = bService;
        }

        public string SayHi() => "Hello from Chicken";

        public string EggSayHi() => s.SayHi();
    }

    public interface IChickenService
    {
        string SayHi();

        string EggSayHi();
    }

}
