using IWeatherApp.Models;
using IWeatherApp.Services;
using System.Net.NetworkInformation;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IWeatherApp
{

    /// <summary>
    ///     Nom    : ISAAC 
    ///     Prenom : Sammuel Ramclief
    ///     Cours  : C# II
    ///     Devoir I
    ///     Description : Application mobile developpée avec .Net MAUI et l'API OpenWeather.
    ///                   Le projet n'utilise pas le système de design MVVM vu que cela n'a pas été demandé.
    ///                   Toutefois des repertoires Models et Services ont été créés pour rendre le code plus clair.
    ///                   
    ///                   Models    |
    ///                             |--> WeatherInfos.cs
    ///                   
    ///                   
    ///                   Services  |
    ///                             |--> GetWeatherService.cs
    /// </summary>
    

    public partial class MainPage : ContentPage
    {

        public WeatherInfos weatherInfos;

        public MainPage()
        {
            InitializeComponent();

            /// App title
            //  Le I prefixant WeatherApp est pour utiliser mon nom
            Title = "IWeatherApp";

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
            string formattedDate = currentDate.ToString("ddd, dd MMM yyyy");

            theDate.Text = formattedDate;

            //Pour formater la temperature
            theTemp.Text += " °C";

            //Pour formater le label Feels like
            theFeelsLike.Text += $"\n{weatherInfos.main.feels_like} °C";

            //Pour formater le label Wind
            //  Vu qu'on utilise &units=mtric pour avoir la température en celcius
            //  speed doit etre multiplié par 3.6 pour avoir la vitesse en km/h et non en m/s
            double theSpeed = weatherInfos.wind.speed * 3.6;
            theSpeed = Math.Floor(theSpeed * 100) / 100;
            theWind.Text += $"\n{theSpeed} km/h";

            //Pour formater le label Humidity
            theHumidity.Text += $"\n{weatherInfos.main.humidity}%";


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
