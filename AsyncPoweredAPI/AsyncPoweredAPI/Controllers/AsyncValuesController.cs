using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camunda.Api.Client;

namespace AsyncPoweredAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsyncValuesController : ControllerBase
    {
        // POST api/values
        //This should really return the business Key and have a get to allow retrieval of the current status.
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var businessKey = Guid.NewGuid();
            CamundaClient camunda = CamundaClient.Create("http://localhost:8080/engine-rest");
            var instance = new Camunda.Api.Client.ProcessDefinition.StartProcessInstance();
            instance.BusinessKey = businessKey.ToString();
            instance.SetVariable("ExternalTaskName", "DoAsyncWork");
            instance.SetVariable("ExternalCallbackTaskName", "AsyncWork_Completed");
            instance.SetVariable("AsyncPostRequest", value);
            camunda.ProcessDefinitions.ByKey("AsyncWebAPI").StartProcessInstance(instance);
        }

    }
}