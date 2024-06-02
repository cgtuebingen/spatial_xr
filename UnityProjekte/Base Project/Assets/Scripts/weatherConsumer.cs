using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weatherConsumer : MonoBehaviour {
    [SerializeField] private float lat = 42.2f;
    [SerializeField] private float lon = 12.0f;
    public WeatherGetter weatherGetter;
    currentWeather weather;
    bool printed = false;
    // Start is called before the first frame update
    void Start() {
        //set random example location
        weatherGetter.updateLocation(lat,lon);
    }

    // Update is called once per frame
    void Update() {
        currentWeather weather = weatherGetter.getWeather();
        if (weather == null) Debug.Log("Waiting for request");
        else {
            if(!printed){
                Debug.Log("Weather from ID:" + weather.weatherType);
                Debug.Log("Temp:" + weather.temp);
                Debug.Log(weather.description);
                printed = true;
            }


        }
    }

}

