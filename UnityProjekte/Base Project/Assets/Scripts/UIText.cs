using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class exampleUIText : MonoBehaviour
{
    public WeatherGetter weatherGetter;
    //this is where the temperature will be shown, place me wherever
    public TMP_Text temperatureText;
    //this is where the temperature will be shown, place me wherever
    public TMP_Text friendlyNameText;
    //name of the city
    public TMP_Text cityText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentWeather weather = weatherGetter.getWeather();
        if(weather != null){
            temperatureText.text = weather.temp + " °C";
            friendlyNameText.text = weatherToFriendlyString(weather.weatherType);
            cityText.text = weather.city;
        }
        else{
            temperatureText.text = "";
            friendlyNameText.text = "";
            cityText.text = "Laden ...";
        }

    }

    string weatherToFriendlyString(currentWeather.WeatherType weather){
        switch(weather){
            case currentWeather.WeatherType.RAIN:
            return "Regen";
            case currentWeather.WeatherType.SUN:
            return "Sonne";
            case currentWeather.WeatherType.SNOW:
            return "Schnee";
            case currentWeather.WeatherType.CLOUDS:
            return "Wolken";
            //in case of new types of weather, add them here, eg. 
            default:
            return "Todo: Implement";
        }

    }
}
