using Json.Protector.Converter;
using Newtonsoft.Json;

namespace Json.Protector.Sample
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public JsonProtectorType? Summary { get; set; }
    }

    public class WeatherForecastWithOutEncrypt
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? SummaryText { get; set; }
    }

    public class WeatherForecastNewtonsoftDataAttribute
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [JsonConverter(typeof(NewtonsoftDataProtector))]
        public string? Summary { get; set; }
        public string? Summary2 { get; set; }
    }


}
