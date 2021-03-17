using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    interface IWeatherService
    {
        IEnumerable<ClimateModel> FindDaysTempPredictedAbove20(string cityName);

        IEnumerable<ClimateModel> FindDaysWeatherPredictedSunny(string cityName);

    }
}
