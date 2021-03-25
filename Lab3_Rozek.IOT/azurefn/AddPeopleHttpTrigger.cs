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
    public static class AddPeopleHttpTrigger
    {
        [FunctionName("AddPeopleHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string firstName = req.Query["firstName"];
            string lastName = req.Query["lastName"];
            string phoneNumber = req.Query["phoneNumber"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            firstName = firstName ?? data?.firstName;
            lastName = lastName ?? data?.lastName;
            phoneNumber = phoneNumber ?? data?.phoneNumber;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber)) {
                string responseMessage = $"Invalid request. firstName: {firstName}, lastName: {lastName}, phoneNumber: {phoneNumber}";
                return new BadRequestObjectResult(responseMessage);
            } else {
                try
                {
                    string connectionString = Environment.GetEnvironmentVariable("PersonDb");
                    log.LogInformation(connectionString);
                    var db = new DatabaseContext(connectionString);
                    int personId = db.AddPeople(firstName, lastName, phoneNumber);
                    return new OkObjectResult($"Person has been added to database with id: {personId}");
                }
                catch (Exception ex)
                {
                    log.LogError(ex, ex.Message);
                    return new JsonResult(ex);
                }
            }
        }
    }
}
