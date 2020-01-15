using Camunda.Api.Client;
using Camunda.Api.Client.ExternalTask;
using IO.Swagger.Api;
using IO.Swagger.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace SideCar
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Get config for the sidecar from the WebAPI it is bridging to.  http://localhost:5000/api/camundaconfig
            var topicConfigs = FetchCamundaConfigs("http://localhost:5000");
            CamundaClient camunda = CamundaClient.Create("http://localhost:8080/engine-rest");
            while (true)
            {
                var topicList = new FetchExternalTasks()
                {
                    AsyncResponseTimeout = 10000,
                    MaxTasks = 50,
                    Topics = new List<FetchExternalTaskTopic>() ,
                    WorkerId = "myworkerId"
                };
                foreach(var config in topicConfigs)
                {
                    topicList.Topics.Add(new FetchExternalTaskTopic(config.Key, 10));
                }
                
                var allTasks = camunda.ExternalTasks.FetchAndLock(topicList).Result;
                Console.WriteLine($"Number of Tasks:{allTasks.Count}");
                foreach (var task in allTasks)
                {
                    var realtask = camunda.ExternalTasks[task.Id].Get().Result;

                    var response = CallTheSyncMethod(task.Variables["AsyncPostRequest"], topicConfigs[task.TopicName]);
                    
                    CompleteTask(camunda, task, response);
                }

                Thread.Sleep(5000);
            }
        }

        private static void CompleteTask(CamundaClient camunda, LockedExternalTask task, string response)
        {
            var completion = new CompleteExternalTask();
            completion.WorkerId = task.WorkerId;
            completion.SetVariable("AsyncPostResponse", response);
            camunda.ExternalTasks[task.Id].Complete(completion);
            Console.WriteLine($"Finsihed Processing {task.Id} for {task.WorkerId}");
        }

        private static Dictionary<string,string> FetchCamundaConfigs(string configURL)
        {
            var httpclient = new HttpClient();
            var a = new CamundaConfigApi(configURL);
            var b = a.GetAsync().Result;
            Dictionary<string, string> result;

            result = new Dictionary<string, string>();
            foreach (var item in b)
            {
                result[$"{item.Key}"] = item.Value;
            }
            return result;
        }

        private static string CallTheSyncMethod(VariableValue variableValue, string url)
        {
            HttpClient httpClient = new HttpClient();
            var requestValue = (string)variableValue.Value;
            var content = new StringContent($"\"{requestValue}\"", Encoding.UTF8, "application/json");
            
            var response = httpClient.PostAsync(url, content).Result;
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
