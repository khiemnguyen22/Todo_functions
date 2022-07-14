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

namespace functions.TodoFunctions
{

    public static class PostTodos
    {
        private const string Route = "postTodo";
        private const string container = "todos";
        private const string userContainer = "users";

        [FunctionName("PostTodos")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
            [Blob(container, Connection = "AzureWebJobsStorage")] BlobContainerClient todoContainer,
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

        [FunctionName("PostAuthTodos")]
        public static async Task<IActionResult> CreateAuthTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route +"/{userId}")] HttpRequest req,
            [Blob(userContainer + "/{userId}.json", Connection = "AzureWebJobsStorage")] string json,
            [Blob(container, Connection = "AzureWebJobsStorage")] BlobContainerClient todoContainer,
            ILogger log, string userId)
        {

            log.LogInformation("Creating a new todo list item");
            await todoContainer.CreateIfNotExistsAsync();
            if (json == null)
            {
                log.LogInformation($"Item {userId} not found");
                return new NotFoundResult();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var inputUser = JsonConvert.DeserializeObject<User>(json);
            var input = JsonConvert.DeserializeObject<UserAuthTodo>(requestBody);
            var todo = new UserAuthTodo() 
            { 
                Description = input.Description,
                UserId = inputUser.Id,
            };

            var blob = todoContainer.GetBlobClient($"{todo.Id}.json");
            await blob.UploadTextAsync(JsonConvert.SerializeObject(todo));

            return new OkObjectResult(todo);
        }

    }
}
