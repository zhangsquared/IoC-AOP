using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CircularReference.Services
{
    public class EggService : IEggService
    {
        private readonly IChickenService s;

        public EggService(IChickenService aService)
        {
            s = aService;
        }

        public string SayHi() => "Hello from Egg";

        public string ChickenSayHi() => s.SayHi();
    }

    public interface IEggService
    {
        string SayHi();

        string ChickenSayHi();
    }

}
