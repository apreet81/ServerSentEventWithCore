using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerSentEventWithCore
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task GetStream()
        {
            var response = _httpContextAccessor.HttpContext.Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.StatusCode = 200;

            for (var i = 0; true; ++i)
            {

                if (_httpContextAccessor.HttpContext.RequestAborted.IsCancellationRequested == true)
                {
                    break;
                }
                await response
                    .WriteAsync($"data: Controller {i} at {DateTime.Now}\r\r");

                response.Body.Flush();
                await Task.Delay(5 * 1000);
            }
        }

        [HttpGet]
        public async Task GetMultipleStream(int sourceID)
        {
            var response = _httpContextAccessor.HttpContext.Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.StatusCode = 200;

            for (var i = 0; true; ++i)
            {

                if (_httpContextAccessor.HttpContext.RequestAborted.IsCancellationRequested == true)
                {
                    break;
                }

                // Need to investigate approach for sending json data.
                //await response.WriteAsJsonAsync(new { Datetime = DateTime.Now, SourceID = sourceID, IterationNumber = i }, new JsonSerializerOptions { }, "text/event-stream");
                //var json = JsonConvert.SerializeObject(new { Datetime = DateTime.Now, SourceID = sourceID, IterationNumber = i });
                //await response.WriteAsync(json);

                await response
                    .WriteAsync($"data: Controller {i} at {DateTime.Now} with source ID : {sourceID}\r\r");

                response.Body.Flush();
                await Task.Delay(5 * 1000);
            }
        }
    }
}
