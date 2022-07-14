using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace functions.TodoFunctions
{
    public static class GetUsers
    {
        private const string Route = "getUsers";
        private const string container = "users";

        [FunctionName("GetUsers")]
        public static async Task<IActionResult> GetAllUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
            [Blob(container, Connection = "AzureWebJobsStorage")] BlobContainerClient userContainer,
            ILogger log)
        {
            log.LogInformation("Getting list of users");
            await userContainer.CreateIfNotExistsAsync();

            var users = new List<User>();
            await foreach (var result in userContainer.GetBlobsAsync())
            {
                var blob = userContainer.GetBlobClient(result.Name);
                var json = await blob.DownloadTextAsync();
                users.Add(JsonConvert.DeserializeObject<User>(json));
            }
            return new OkObjectResult(users);
        }
    }
}
