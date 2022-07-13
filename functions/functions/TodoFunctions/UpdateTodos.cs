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

namespace functions.TodoFunctions
{

    public static class UpdateTodos
    {
        private const string Route = "updateTodo";
        private const string container = "todos";
        [FunctionName("UpdateTodos")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route + "/{id}")] HttpRequest req,
            [Blob(container + "/{id}.json", Connection = "AzureWebJobsStorage")] BlobClient blob,
            ILogger log, string id)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<Todo>(requestBody);

            if (!await blob.ExistsAsync())
            {
                return new NotFoundResult();
            }
            var existingText = await blob.DownloadTextAsync();
            var existingTodo = JsonConvert.DeserializeObject<Todo>(existingText);

            existingTodo.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrEmpty(updated.Description))
            {
                existingTodo.Description = updated.Description;
            }

            await blob.UploadTextAsync(JsonConvert.SerializeObject(existingTodo));

            return new OkObjectResult(existingTodo);
        }
    }
}
