using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Json.Protector.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;


        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet]
        [Route("GetNewtonsoftDataAttributeWeatherForecast")]
        public IEnumerable<WeatherForecastNewtonsoftDataAttribute> GetNewtonsoftDataAttributeWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastNewtonsoftDataAttribute
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Summary2 = "Test" + Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        }

        [HttpPost]
        [Route("PostWeatherForecast")]
        public IEnumerable<WeatherForecastWithOutEncrypt> Post(IEnumerable<WeatherForecast> list)
        {
            return list.Select(c => new WeatherForecastWithOutEncrypt
            {
                Date = c.Date,
                TemperatureC = c.TemperatureC,
                SummaryText = c.Summary,

            }).ToList();
        }

        [HttpPost]
        [Route("PostWeatherForecastAttribute")]
        public IEnumerable<WeatherForecastWithOutEncrypt> PostWeatherForecastAttribute(IEnumerable<WeatherForecastNewtonsoftDataAttribute> list)
        {
            var result = list.Select(c => new WeatherForecastWithOutEncrypt
            {
                Date = c.Date,
                TemperatureC = c.TemperatureC,
                SummaryText = c.Summary.ToString(),

            }).ToList();

            return result;
        }



        [HttpGet]
        [Route("GeUserProfile")]
        public UserProfile GeUserProfile()
        {
            var profile = new UserProfile
            {
                Name = "saied rahimi",
                SensitiveInfo = "This is encrypted data"
            };

            return profile;
        }

        [HttpPost]
        [Route("PostUserProfile")]
        public UserProfileWithOutEncrypt PostUserProfile(UserProfile model)
        {
            return new UserProfileWithOutEncrypt
            {
                Name = model.Name,
                Message = model.SensitiveInfo,

            };
        }


        [HttpPost]
        [Route("PostUserProfileAttribute")]
        public UserProfileWithOutEncrypt PostUserProfileAttribute(UserProfileAttribute model)
        {
            return new UserProfileWithOutEncrypt
            {
                Name = model.Name,
                Message = model.SensitiveInfo,

            };
        }

    }
}
