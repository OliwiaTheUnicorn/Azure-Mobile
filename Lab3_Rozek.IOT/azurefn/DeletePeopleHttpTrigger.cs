using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cdv.People
{
    public static class DeletePeopleHttpTrigger
    {
        [FunctionName("DeletePeopleHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string personId = req.Query["personId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            personId = personId ?? data?.personId;
            int id = 0;
            if (Int32.TryParse(personId, out id))
            {
                try
                {
                    string connectionString = Environment.GetEnvironmentVariable("PersonDb");
                    log.LogInformation(connectionString);
                    var db = new DatabaseContext(connectionString);
                    db.DeletePeople(id);
                    return new OkObjectResult($"Person with id: {personId} has been deleted");
                }
                catch (Exception ex)
                {
                    log.LogError(ex, ex.Message);
                    return new JsonResult(ex);
                }
            } else {
                string responseMessage = $"Invalid request. personId: {personId} is invalid";
                return new BadRequestObjectResult(responseMessage);
            }
        }
    }
}
