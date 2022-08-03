using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ButtPillowCDS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherUpdateController : ControllerBase
    {

        private readonly ILogger<WeatherUpdateController> _logger;

        public WeatherUpdateController(ILogger<WeatherUpdateController> logger)
        {
            _logger = logger;
        }

    }
}
