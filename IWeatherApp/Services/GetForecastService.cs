using IWeatherApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IWeatherApp.Services
{



    internal class GetForecastService
    {
        private const string _openWeatherApiKey = "8dd02c7c268f4f64e6662599ef187d4c";
        private const string _baseUrl = "https://api.openweathermap.org/data/2.5/forecast";

        public async static Task<ForecastInfos> getForecastInfos()
        {
            ForecastInfos forecastInfos;

            using (var httpClient = new HttpClient())
            {
                try
                {


                    //TO-DO : Check steps from Weather Service
                    string ville = "Port-au-Prince";

                    //Selon la documentation la version gratuite de l'API donne un forecast pour 5 jours avec 3 heures d'intervalles
                    //Nous utilisons 16 pour avoir une marge
                    int cnt = 16;
                    

                    // Selon la documentation d'openWeather, &units=metric permet d'obtenir la temperature en celcius
                    string siteLink = $"{_baseUrl}?q={ville}&cnt={cnt}&appid={_openWeatherApiKey}&units=metric";

                    var response = await httpClient.GetAsync(siteLink);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();


                        forecastInfos = JsonSerializer.Deserialize<ForecastInfos>(jsonResponse);



                        return forecastInfos;
                    }
                    else
                    {
                        //To-DO : ....
                        throw new HttpRequestException($"{response.StatusCode} - HTTP REQUEST EXCEPTION");
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Une erreur s'est produite !", ex);
                }
            }

        }
    }
}
