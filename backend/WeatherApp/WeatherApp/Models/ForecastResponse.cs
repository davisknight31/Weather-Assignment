namespace WeatherApp.Models
{
    public class ForecastResponse
    {
        public Forecast forecast { get; set; }
    }

    public class Forecast
    {
        public List<ForecastDay> forecastDays { get; set; }
    }

    public class ForecastDay { 
        public DateTime date { get; set; }
    }

}
