using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using functions.Helpers;
using functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace functions.UserFunctions
{
    public static class PostUsers
    {
        private const string Route = "postUser";
        private const string container = "users";

        [FunctionName("PostUsers")]
        public static async Task<IActionResult> CreateUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
            [Blob(container, Connection = "AzureWebJobsStorage")] BlobContainerClient userContainer,
            ILogger log)
        {

            log.LogInformation("Creating a new user");
            await userContainer.CreateIfNotExistsAsync();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<User>(requestBody);
            var user = new User() {
               FirstName  = input.FirstName,
               Email = input.Email,
               Password = input.Password,
            };

            var blob = userContainer.GetBlobClient($"{user.Id}.json");
            await blob.UploadTextAsync(JsonConvert.SerializeObject(user));

            return new OkObjectResult(user);
        }
    }
}
