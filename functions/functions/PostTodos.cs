using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

    public static class PostTodos
    {
        private const string Route = "postTodo";
        private const string BlobPath = "todos";

        [FunctionName("PostTodos")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
            [Blob(BlobPath, Connection = "AzureWebJobsStorage")] BlobContainerClient todoContainer,
            ILogger log)
        {

            log.LogInformation("Creating a new todo list item");
            await todoContainer.CreateIfNotExistsAsync();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<Todo>(requestBody);
            var todo = new Todo() { Description = input.Description };

            var blob = todoContainer.GetBlobClient($"{todo.Id}.json");
            await blob.UploadTextAsync(JsonConvert.SerializeObject(todo));

            return new OkObjectResult(todo);
        }
    }
}
