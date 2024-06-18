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

        if(timeString.Equals("")){
            time = DateTime.Now;
        }
        else{
            time = DateTime.ParseExact(timeString,"yyyy-MM-dd",CultureInfo.InvariantCulture);
        }
        weatherGetter.setRequestTime(time);
        //weatherGetter.updateLocation(lat,lon);
        weatherGetter.updateLocationFromString("Tübingen");
    }

    // Update is called once per frame
    void Update() {
        WeatherGetter.Result<WeatherResult>? weather = weatherGetter.getWeather();
        if (weather is WeatherGetter.Result<WeatherResult> weatherRes ) {
            if(!weatherRes.isOk) Debug.Log("Error");
            else if(!printed){
                Debug.Log("Weather from ID:" + weatherRes.value.weatherType);
                Debug.Log("Temp:" + weatherRes.value.temp);
                Debug.Log(weatherRes.value.city);
                printed = true;
            }
        }
        else Debug.Log("Waiting for request");


        
    }

}

