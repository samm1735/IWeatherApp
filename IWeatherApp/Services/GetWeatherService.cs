using IWeatherApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IWeatherApp.Services
{
    internal class GetWeatherService
    {
        private const string _openWeatherApiKey = "8dd02c7c268f4f64e6662599ef187d4c";
        private const string  _baseUrl = "https://api.openweathermap.org/data/2.5/weather";


        public async static Task<WeatherInfos> getWeatherInfos()
        {
            WeatherInfos weatherInfos;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    

                    //TO-DO : Check example steps from S. Poteau
                    string ville = "Port-au-Prince";
                    //ville = Uri.EscapeDataString(ville);

                    // Selon la documentation d'openWeather, &units=metric permet d'obtenir la temperature en celcius
                    string siteLink = $"{_baseUrl}?q={ville}&appid={_openWeatherApiKey}&units=metric";

                    var response = await httpClient.GetAsync(siteLink);

                    if(response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
    

                        weatherInfos = JsonSerializer.Deserialize<WeatherInfos>(jsonResponse);

                        

                        return weatherInfos;
                    }
                    else
                    {
                        //To-DO : ....
                        throw new HttpRequestException($"{response.StatusCode} - HTTP REQUEST EXCEPTION");
                    }

                }
                catch(Exception ex)
                {
                    throw new Exception("Une erreur s'est produite !", ex);
                }
            }

            // A enlever apres avoir terminer la definition de cete methode
            //return weatherInfos;
        }

    }
}
