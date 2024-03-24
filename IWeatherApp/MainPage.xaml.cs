using IWeatherApp.Models;
using IWeatherApp.Services;
using System.Net.NetworkInformation;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IWeatherApp
{

    /// <summary>
    /// 
    ///     Nom         : ISAAC 
    ///     Prenom      : Sammuel Ramclief
    ///     Cours       : C# II
    ///     Devoir I
    ///     Description : Application mobile developpée avec .Net MAUI et l'API OpenWeather.
    ///                   Le projet n'utilise pas le système de design MVVM vu que cela n'a pas été demandé.
    ///                   
    ///                   Toutefois des repertoires Models et Services ont été créés pour rendre le code plus clair.
    ///                   
    ///                   Models    |
    ///                             |--> WeatherInfos.cs    //Pour le climat actuel
    ///                             |--> ForecastInfos.cs   //Pour le climat du lendemain
    ///                   
    ///                   
    ///                   Services  |
    ///                             |--> GetWeatherService.cs    //Pour le climat actuel
    ///                             |--> GetForecastService.cs  //Pour le climat du lendemain
    ///                             
    /// 
    ///     API utilisé : OpenWeatherMap
    ///                 Liens :
    ///                         --> Current Weather : https://openweathermap.org/current
    ///                         --> 5 Days/3 hours forecast : https://openweathermap.org/forecast5
    ///                         
    /// 
    /// </summary>


    public partial class MainPage : ContentPage
    {

        public WeatherInfos weatherInfos; //Pour le climat actuel

        public ForecastInfos forecastInfos; // Pour le climat futur

        public MainPage()
        {
            InitializeComponent();

            Title = ""; // Pour effacer le titre "Home" qui vient de base

            //Si l'appereil n'est pas connecté à internet nous verrons un ecran pardefaut

            if(IsInternetAvailable())
            {
                GetDataToUI();
            }
            else
            {
                GetDefaultUI();
            }

            

            
        }

        public void GetDefaultUI()
        {
            //Un icone par defaut sera affiche dans ce cas
            theCity.Text = "No Internet connection";
        }

        private async void GetDataToUI()
        {

            weatherInfos = await GetWeatherService.getWeatherInfos();

            BindingContext = weatherInfos;


            ////
            ///     La ligne suivante prend le string representant :
            ///         le nom de l'icon associe au climat actuel
            ///         @4x.png c'est pour la taille de l'image
            ///         
            ///     Documentation : https://openweathermap.org/weather-conditions
            ////
            weatherStatusIcon.Source = $"https://openweathermap.org/img/wn/{weatherInfos.weather[0].icon}@4x.png";



            ////
            //  Les lignes suivantes convertisses le timestamp obtenu en Datetime.
            //  Cet objet Datetime est ensuite formaté pour permettre l'affichage suivant l'UI de référence fourni
            ////

            long unixTimestamp = weatherInfos.dt;
            DateTime currentDate = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;

            // Format the DateTime object as a string
            string formattedDate = currentDate.ToString("dddd, dd MMM yyyy");

            theDate.Text = formattedDate;




            //Pour formater la temperature
            theTemp.Text += " °C";

            //Pour formater le label Feels like
            theFeelsLike.Text += $"\n{weatherInfos.main.feels_like} °C";





            //Pour formater le label Wind
            //  Selon la documentation l'API dnne la vitesse en m/s
            //  speed doit etre multiplié par 3.6 pour avoir la vitesse en km/h et non en m/s
            double theSpeed = weatherInfos.wind.speed * 3.6;
            theSpeed = Math.Floor(theSpeed * 100) / 100;
            theWind.Text += $"\n{theSpeed} km/h";

            //Pour formater le label Humidity
            theHumidity.Text += $"\n{weatherInfos.main.humidity}%";


            ///
            /// Les lignes suivantes sont pour le forecast
            ///


            forecastInfos = await GetForecastService.getForecastInfos();


            // Les variables suivantes seront utilises pour avoir la moyenne de temperature pour le lendemain

            float combinedForecastTemp = 0, combinedForecastFeelsLike = 0;

            // Pour avoir la date du lendemain
            DateTime tomorrow = DateTime.Today.AddDays(1);


            Dictionary<string, int> weatherCount = new Dictionary<string, int>();

            // Pour filtrer a travers les forecast du lendemain
            foreach (var forecast in forecastInfos.list)
            {


                long forecastUnixTimestamp = forecast.dt;
                DateTime forecastDateTime = DateTimeOffset.FromUnixTimeSeconds(forecastUnixTimestamp).DateTime;

                if (forecastDateTime.Date == tomorrow)
                {
                    //Dans les lignes suivantes nous additionnons les temperatures et les les "feels like" ensemble
                    //  Plus tard ce sera utilisé pour avoir la moyenne de ces valeurs pour la journee entière
                    combinedForecastTemp += forecast.main.temp;
                    combinedForecastFeelsLike += forecast.main.feels_like;



                    //Dans les lignes suivantes nous verifions les icones de chaque forecast
                    // Ainsi nous saurons quel icon sera le plus utilisé pour le lendemain

                    string forecastIcon = forecast.weather[0].icon;

                    if (!weatherCount.ContainsKey(forecastIcon))
                    {
                        weatherCount[forecastIcon] = 0;
                    }
                    weatherCount[forecastIcon]++;


                }

            }

            // Find the most frequent weather condition for tomorrow
            KeyValuePair<string, int> mostFrequentForecastIcon = new KeyValuePair<string, int>("", 0);
            foreach (var icon in weatherCount)
            {
                if (icon.Value > mostFrequentForecastIcon.Value)
                {
                    mostFrequentForecastIcon = icon;
                }
            }

            // Nous utilisons l'icône de climat le plus répété dans la réponse de l'API comme icône de climat globale pour le lendemain.
            string overallForecastIcon = mostFrequentForecastIcon.Key;




            //Nou divisions par 8 car selon la documentation nous aurons des intervalles de 3 h, pour 24 h cela fait 8 forecasts.

            combinedForecastTemp /= 8;
            combinedForecastFeelsLike /= 8;

            

            // Pour les affichage sur l'ecran

            theForecastDate.Text = tomorrow.ToString("ddd");

            theForecastTemps.Text = $"{(int)combinedForecastTemp}°/{(int)combinedForecastFeelsLike}°";

            

            forecastStatusIcon.Source = $"https://openweathermap.org/img/wn/{overallForecastIcon}@4x.png";
        
        
        
        }

            /// <summary>
            ///     Check if the device is connected to the internet
            /// </summary>
            /// <returns> True or Flase </returns>
            public static bool IsInternetAvailable()
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    return false; // No Internet
                }

                try
                {
                    using (var client = new System.Net.WebClient())
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true; // Internet Available
                    }
                }
                catch
                {
                    return false; // En cas d'Exception return false
                }
            }


}

}
