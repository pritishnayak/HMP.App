using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hmp.App.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Hmp.App.Api
{
    public static class WeatherForecastFunction
    {
        private const string FunctionName = "WeatherForecast";
        private static string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing";
            }

            return summary;
        }

        [Function("WeatherForecast")]
        public static async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(FunctionName);
            logger.LogDebug("{FunctionName} is invoked.", FunctionName);

            var randomNumber = new Random();
            var temp = 0;

            var forecastData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temp = randomNumber.Next(-20, 55),
                Summary = GetSummary(temp)
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteAsJsonAsync(forecastData);

            return response;
        }
    }

    //public static class Function1
    //{
    //    [Function("Function1")]
    //    public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
    //        FunctionContext executionContext)
    //    {
    //        var logger = executionContext.GetLogger("Function1");
    //        logger.LogInformation("C# HTTP trigger function processed a request.");

    //        var response = req.CreateResponse(HttpStatusCode.OK);
    //        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

    //        response.WriteString("Welcome to Azure Functions!");

    //        return response;
    //    }
    //}
}
