using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AsyncPoweredAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        ILoggerFactory _loggerFactory;

        public ClientController(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        // POST: api/Client
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var logger = _loggerFactory.CreateLogger("clientAPILogger");
            logger.LogError(value);

        }

    }
}
