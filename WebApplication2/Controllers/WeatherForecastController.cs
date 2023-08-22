using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;


[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;

    public WeatherForecastController(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<string>> Get()
    {



        Console.WriteLine("Got in WebApi1");
        if (_httpContextAccessor.HttpContext != null)
        {
            string requestingUrl = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
            string logEntry = $"Request from: {requestingUrl} at {DateTime.Now}";
            await System.IO.File.AppendAllLinesAsync("log1.txt", new[] { logEntry });
        }
        else

        {
            Console.WriteLine("HttpContext is null!");
        }

        // System.IO.File.AppendAllLines("log1.txt", new[] { "Get called at " + DateTime.Now + " " +Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User) });

        System.Threading.Thread.Sleep(400);
        await CallOtherApi();
        return new[]
            {
             "moi!"
            };

    }

    [HttpGet("CallOtherApi")]
    public async Task<IActionResult> CallOtherApi()
    {
        string apiUrl = "https://localhost:7282/WeatherForecast";    // Replace with your WebApplication2 URL

        // Log the request information
        string logEntry = $"Calling WebApplication2 API at {apiUrl} from {Request.GetDisplayUrl()} at {DateTime.Now}";
        await System.IO.File.AppendAllLinesAsync("log.txt", new[] { logEntry });

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            return Ok(apiResponse);
        }
        else
        {
            return StatusCode((int)response.StatusCode);
        }
    }



};
