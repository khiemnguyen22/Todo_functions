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

    public static class GetTodosById
    {
        private const string Route = "getTodoById";
        private const string container = "todos";

        [FunctionName("GetTodosById")]
        public static IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route + "/{id}")] HttpRequest req,
            [Blob(container + "/{id}.json", Connection = "AzureWebJobsStorage")] string json,
            ILogger log, string id)
        {
            log.LogInformation("Getting todo item by id");
            if (json == null)
            {
                log.LogInformation($"Item {id} not found");
                return new NotFoundResult();
            }
            return new OkObjectResult(JsonConvert.DeserializeObject<Todo>(json));
        }
    }
}