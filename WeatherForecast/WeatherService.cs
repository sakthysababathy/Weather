using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace WeatherForecast
{
    class WeatherService : IWeatherService
    {

        private readonly IConfiguration configuration;

        private const string FORECAST_RESOURCE = "data/2.5/forecast";

        public WeatherService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public List<ClimateModel> GetForecastData(string query)
        {
            List<ClimateModel> result = null;
            var uri = new Uri(configuration.GetSection("API_BASE_URL").Value);
            var client = new RestClient(uri);
            var request = new RestRequest(FORECAST_RESOURCE, Method.GET);
            request.AddParameter("q", query);
            request.AddParameter("appid", configuration.GetSection("API_KEY").Value);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject respObj = JObject.Parse(response.Content);
                JArray list = (JArray)respObj.SelectToken("$.list");
                result = new List<ClimateModel>();
                foreach (var item in list)
                {
                    result.Add(new ClimateModel(item));
                }
            }
            return result;
        }
     

        public IEnumerable<ClimateModel> FindDaysTempPredictedAbove20(string cityName)
        {
            DateTime startDateTime = DateTime.Now.Date.AddDays(1);

            DateTime endDateTime = startDateTime.AddDays(5).AddMilliseconds(-1);
            List<ClimateModel> result = new List<ClimateModel>();
            List<ClimateModel> forecastData = GetForecastData(cityName);
            if (forecastData != null)
            {
                forecastData = forecastData.FindAll(n => n.temp > 20);

                foreach (var item in forecastData)
                {
                    if (endDateTime >= item.dateTime  && startDateTime <= item.dateTime && !result.Exists(n => n.dateTime.Date == item.dateTime.Date))
                        result.Add(item);
                }
            }
            return result;
        }

        public IEnumerable<ClimateModel> FindDaysWeatherPredictedSunny(string cityName)
        {

            DateTime startDateTime = DateTime.Now.Date.AddDays(1);
            DateTime endDateTime = startDateTime.AddDays(5).AddMilliseconds(-1);

            List<ClimateModel> result = new List<ClimateModel>();
            List<ClimateModel> forecastData = GetForecastData(cityName);
            if (forecastData != null)
            {
                List<ClimateModel> clearForecastData = forecastData.FindAll(n => n.climates.FindAll( p => !p.Equals("Clear")).Count == 0);
                foreach (var item in clearForecastData)
                {
                    if (endDateTime >= item.dateTime && startDateTime <= item.dateTime && !result.Exists(n => n.dateTime.Date == item.dateTime.Date))
                    {
                        if (forecastData.FindAll(n => n.dateTime.Date == item.dateTime.Date).Count == 
                            clearForecastData.FindAll(n => n.dateTime.Date == item.dateTime.Date).Count)
                            result.Add(item);
                    }
                }
            }
            return result;
        }
    }
}
