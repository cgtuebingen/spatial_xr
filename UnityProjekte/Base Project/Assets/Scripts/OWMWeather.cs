using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWMWeather : WeatherResult
{
    public OWMWeather(double temp, int id, string city):base(temp - 273.15,parseWeatherFromID(id),city) {
    }    
   //Based on: https://openweathermap.org/weather-conditions
    public static WeatherResult.WeatherType parseWeatherFromID(int id){
        int firstDigit = id/100;
        switch (firstDigit){
            //Thunderstorm
            case 2:
            //Drizzle
            case 3:
            //Rain
            case 5:
            return WeatherType.RAIN;
            break;
            //Snow
            case 6:
            return WeatherType.SNOW;
            break;
            //Atmosphere
            case 7:
            case 8:
            if(id == 800) return WeatherType.SUN;
            return WeatherType.CLOUDS;
            break;
            //this should never happen, we just pretend nothing is wrong
            default:
            return WeatherType.SUN;
        }
    }
}
