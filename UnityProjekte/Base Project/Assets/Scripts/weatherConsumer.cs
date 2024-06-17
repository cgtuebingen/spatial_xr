using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
public class weatherConsumer : MonoBehaviour {
    [SerializeField] private float lat = 42.2f;
    [SerializeField] private float lon = 12.0f;
    [SerializeField] public string timeString = "";
    private DateTime time;
    public WeatherGetter weatherGetter;
    WeatherResult weather;
    bool printed = false;
    // Start is called before the first frame update
    void Start() {
        //set random example location

        if(timeString.Equals("") && false){
            time = DateTime.Now;
        }
        else{
            time = DateTime.ParseExact(timeString,"yyyy-MM-dd",CultureInfo.InvariantCulture);
        }
        weatherGetter.setRequestTime(time);
        weatherGetter.updateLocation(lat,lon);
    }

    // Update is called once per frame
    void Update() {
        WeatherResult weather = weatherGetter.getWeather();
        if (weather == null) Debug.Log("Waiting for request");
        else {
            if(!printed){
                Debug.Log("Weather from ID:" + weather.weatherType);
                Debug.Log("Temp:" + weather.temp);
                Debug.Log(weather.city);
                printed = true;
            }


        }
    }

}

