using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CSandun.Todo
{
    public class TodoApiFunction
    {
        private readonly ILogger<TodoApiFunction> _logger;
        public static List<Todo> items = new List<Todo>();

        public TodoApiFunction(ILogger<TodoApiFunction> log)
        {
            _logger = log;
        }

        [FunctionName("CreateTodo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var todo = JsonConvert.DeserializeObject<Todo>(requestBody);
            items.Add(todo);
            _logger.LogInformation("successfully added ");
            return new OkObjectResult(todo);
        }

        [FunctionName("GetAllTodos")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult GetAllTodos(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequest req)
        {
            return new OkObjectResult(items);
        }

        [FunctionName("GetTodoById")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult GetTodoById(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos/{id}")] HttpRequest req, long id)
        {
            var item = items.FirstOrDefault(o => o.Id == id);

            if (item == null)
            {
                return new NotFoundObjectResult("Cannot found todo item");
            }

            return new OkObjectResult(item);
        }
    }
}

