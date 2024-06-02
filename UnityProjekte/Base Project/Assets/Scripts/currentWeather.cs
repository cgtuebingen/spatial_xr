using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class currentWeather {

    //Default Data
    public double temp = 0.0f;
    public int id = 0;
    public WeatherType weatherType;
    public string main;
    public string description = "default";
    public string icon = "###";

    public currentWeather(double temp, int id, string main, string description, string icon) {

        this.temp = temp;
        this.id = id;
        this.main = main;
        this.description = description;
        this.icon = icon;
        weatherType = parseWeatherFromID(id);
    }

    public enum WeatherType{
        Sun,Rain,Clouds,Snow
    }
    //Based on: https://openweathermap.org/weather-conditions
    public WeatherType parseWeatherFromID(int id){
        int firstDigit = id/100;
        switch (firstDigit){
            //Thunderstorm
            case 2:
            //Drizzle
            case 3:
            //Rain
            case 5:
            return WeatherType.Rain;
            break;
            //Snow
            case 6:
            return WeatherType.Snow;
            break;
            //Atmosphere
            case 7:
            case 8:
            if(id == 800) return WeatherType.Sun;
            return WeatherType.Clouds;
            break;
            //this should never happen, we just pretend nothing is wrong
            default:
            return WeatherType.Sun;
        }
    }

}
    
