using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dreidel.Function
{
    public static class DreidelTrigger
    {
        [FunctionName("DreidelTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            Random r = new Random();
            int number = r.Next(1, 4); //for ints

            string spin = "";

            switch (number)
            {
                case 1:
                    spin = "Nun";
                    break;
                case 2:
                    spin = "Gimmel";
                    break;
                case 3:
                    spin = "Hay";
                    break;
                case 4:
                    spin = "Shin";
                    break;
                default:
                    spin = "oops! It didn't work?!";
                    break;
            }

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}, you spun {spin}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
