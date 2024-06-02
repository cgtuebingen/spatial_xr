using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weatherConsumer : MonoBehaviour {
    public WeatherGetter weatherGetter;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
        currentWeather weather = weatherGetter.getCurrentWeather();
        if (weather == null) Debug.Log("Waiting for request");
        else {

            Debug.Log(weather.temp);
            Debug.Log(weather.description);

        }
    }

}

