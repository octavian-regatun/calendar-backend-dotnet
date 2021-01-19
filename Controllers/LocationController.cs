using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using calendar_backend_dotnet.Entities;
using System.Net.Http;
using System.Text.Json;
using calendar_backend_dotnet.Models;

namespace calendar_backend_dotnet.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(string q, string ip)
        {
            const string HERE_URI = "https://autosuggest.search.hereapi.com/v1/autosuggest";
            string HERE_API_KEY = Environment.GetEnvironmentVariable("HERE_API_KEY");

            GpsCoordinates location = await GetLocationFromIpApi(ip);

            string parameters = $"?q={q}&apiKey={HERE_API_KEY}&at={location.Lat},{location.Lon}";

            string response = await App.Http.client.GetStringAsync(HERE_URI + parameters);

            return Ok(response);
        }

        private async Task<GpsCoordinates> GetLocationFromIpApi(string ip)
        {
            string IP_API_URI = $"http://ip-api.com/json/{ip}";

            string response = await App.Http.client.GetStringAsync(IP_API_URI);

            return JsonSerializer.Deserialize<GpsCoordinates>(response);
        }
    }
}
