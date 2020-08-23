using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CircularReference.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CircularReference.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IChickenService chicken;
        private readonly IEggService egg;

        public TestController(IChickenService chickenService, IEggService eggService)
        {
            chicken = chickenService;
            egg = eggService;
        }

        [HttpGet]
        public string DoThings()
        {
            string fromEgg = egg.ChickenSayHi();
            string fromChicken = chicken.SayHi();
            bool res = fromEgg.Equals(fromChicken);
            return res.ToString() + " " + chicken.SayHi();
        }

    }
}
