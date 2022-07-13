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

    public static class DeleteTodos
    {
        private const string Route = "deleteTodo";
        private const string container = "todos";

        [FunctionName("DeleteTodos")]
        public static async Task<IActionResult> DeleteTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route + "/{id}")] HttpRequest req,
        [Blob(container + "/{id}.json", Connection = "AzureWebJobsStorage")] BlobClient blob,
        ILogger log, string id)
        {
            if (!await blob.ExistsAsync())
            {
                return new NotFoundResult();
            }
            await blob.DeleteAsync();
            return new OkResult();
        }
    }
}
