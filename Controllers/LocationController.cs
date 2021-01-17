using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using calendar_backend_dotnet.Entities;
using System.Net.Http;
using System.Text.Json;

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

            GpsCoordinates location = await GetLocationFromIp(ip);

            string parameters = $"?q={q}&apiKey={HERE_API_KEY}&at={location.Lat},{location.Lon}";

            string response = await MyHttp.client.GetStringAsync(HERE_URI + parameters);

            if (String.IsNullOrEmpty(response))
            {
                return StatusCode(404);
            }

            return Ok(response);
        }

        private async Task<GpsCoordinates> GetLocationFromIp(string ip)
        {
            string IP_API_URI = $"http://ip-api.com/json/{ip}";

            string response = await MyHttp.client.GetStringAsync(IP_API_URI);

            return JsonSerializer.Deserialize<GpsCoordinates>(response);
        }
    }
}
