using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPoweredAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamundaConfigController : ControllerBase
    {
        // GET api/camundaconfig
        /// <summary>
        /// This result is a map of camunda.externalTask and workerURL that the sidecar should listen for and the url that we will be posting to.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Dictionary<string, string>> Get()
        {
            var config = new Dictionary<string, string>();

            config.Add("DoAsyncWork", "http://localhost:5000/api/values");
            config.Add("AsyncWork_Completed", "http://localhost:5000/api/client");
            
            return config;
        }

    }
}