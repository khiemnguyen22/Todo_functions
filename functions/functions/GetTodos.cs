using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace functions
{
    public static class GetTodos
    {
        private const string Route = "blobtodo";
        private const string BlobPath = "todos";

        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetAllTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
            [Blob(BlobPath, Connection = "AzureWebJobsStorage")] BlobContainerClient todoContainer,
            ILogger log)
        {
            log.LogInformation("Getting todo list items");
            await todoContainer.CreateIfNotExistsAsync();

            var todos = new List<Todo>();
            await foreach (var result in todoContainer.GetBlobsAsync())
            {
                var blob = todoContainer.GetBlobClient(result.Name);
                var json = await blob.DownloadTextAsync();
                todos.Add(JsonConvert.DeserializeObject<Todo>(json));
            }
            return new OkObjectResult(todos);
        }
    }
}
