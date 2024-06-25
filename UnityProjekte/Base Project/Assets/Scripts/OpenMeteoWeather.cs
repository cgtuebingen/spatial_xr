using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMeteoWeather : WeatherResult
{

    public OpenMeteoWeather(double temp, int id, string city):base(temp,parseWeatherFromID(id),city) {
    }    
   //Based on: https://openweathermap.org/weather-conditions
    public static WeatherResult.WeatherType parseWeatherFromID(int id){
        switch (id){
            case 0:
            return WeatherResult.WeatherType.SUN;
            case 1:
            case 2:
            case 3:
            return WeatherResult.WeatherType.CLOUDS;
            case 71:
            case 73:
            case 75:
            case 77:
            return WeatherResult.WeatherType.SNOW;
            default:
            return WeatherResult.WeatherType.RAIN;

        }
    }
}
