using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public abstract class WeatherResult {

    //Default Data
    //Temperature in °C
    public double temp = 0.0f;
    public WeatherType weatherType;
    public string city;
    public WeatherResult(double temp, WeatherType id, string city) {
        this.temp = temp;
        this.weatherType = id;
        this.city = city;
    }

    public enum WeatherType{
        SUN,RAIN,SNOW,CLOUDS
    }
}
    
