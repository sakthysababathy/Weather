using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WeatherForecast
{
    class ClimateModel
    {
        public DateTime dateTime;

        public double temp;

        public List<string> climates;

        public ClimateModel(JToken item)
        {
            dateTime = Util.UnixTimeStampToDateTime(Convert.ToDouble(item.SelectToken("$.dt")));

            temp = Convert.ToDouble(item.SelectToken("$.main.temp")) - 273.15;

            JArray arr = (JArray)item.SelectToken("$.weather");

            climates = new List<string>();
            foreach (var obj in arr)
            {
                climates.Add(obj.SelectToken("$.main").ToString());
            }
        }
    }
}
