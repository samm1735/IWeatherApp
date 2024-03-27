# Devoir I - Application mobile avec .NET MAUI et OpenWeather API
### Cours
- C# II
  
## Nom et prénom
- ISAAC Sammuel Ramclief

### App Preview
<img src="https://github.com/samm1735/IWeatherApp/blob/main/Screenshot_2024-03-24-17-12-20-877_com.companyname.iweatherapp~2.jpg" alt="App preview" width="200" height="350">


## Description
Application mobile développée avec .Net MAUI et l'API OpenWeather.
Le projet n'utilise pas le système de design MVVM vu que cela n'a pas été demandé.
Toutefois des répertoires Models et Services ont été créés pour rendre le code plus clair.

```csharp
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
```                         

## API utilisée
OpenWeatherMap

### Liens
- [Current Weather](https://openweathermap.org/current)
- [5 Days/3 hours forecast](https://openweathermap.org/forecast5)
