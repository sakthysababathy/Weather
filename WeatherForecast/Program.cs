using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WeatherForecast
{
    class Program
    {
        public static IConfiguration configuration;

        static void Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            IWeatherService weatherService = new WeatherService(configuration);

            string cityName = "Sydney";

            IEnumerable<ClimateModel> daysAbove20 = weatherService.FindDaysTempPredictedAbove20(cityName);

            IEnumerable<ClimateModel> daysPredictedSunny = weatherService.FindDaysWeatherPredictedSunny(cityName);

            Console.WriteLine("No of days temp above 20 degree celcius :: " + daysAbove20.Count());

            Console.WriteLine("No of days weather predicted to be sunny :: " + daysPredictedSunny.Count());

            Console.ReadKey();
        }
    }
}
