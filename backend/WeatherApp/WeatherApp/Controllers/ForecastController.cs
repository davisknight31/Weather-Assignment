using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherApp.Models;
using Newtonsoft.Json;


namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForecastController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ForecastController> _logger;
        private readonly string _url;
        private readonly string _apiKey;
        

        //http://api.weatherapi.com/v1/forecast.json?&q=London&days=1
        public ForecastController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ForecastController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _url = configuration.GetValue<string>("WeatherApi:Url");
            _apiKey = configuration.GetValue<string>("WeatherApi:ApiKey");
        }

        private async Task<string> SendRequest(HttpMethod method, string relativeUrl/* HttpContent content = null*/)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_url);
            httpClient.DefaultRequestHeaders.Add("key", _apiKey);
            var request = new HttpRequestMessage(method, relativeUrl) {/* Content = content*/ };

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;

        }

        [HttpGet("GetWeather/{location}/{days}")]
        public async Task<ActionResult> GetWeather(string location, string days)
        {
            try
            {
                var responseBody = await SendRequest(HttpMethod.Get, $"forecast.json?key=b2944cf156c5494f9ae223216240702&q={location}&days={days}&aqi=no&alerts=no\r\n");

                var stockResponse = JsonConvert.DeserializeObject<ForecastResponse>(responseBody);

                return Ok(responseBody);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
