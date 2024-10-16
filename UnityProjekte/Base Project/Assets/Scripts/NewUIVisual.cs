using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Controller for the UI
public class exampleUIText : MonoBehaviour
{
    public WeatherGetter weatherGetter;
    //this is where the temperature will be shown, place me wherever
    public TMP_Text temperatureText;
    //this is where the temperature will be shown, place me wherever
    public TMP_Text friendlyNameText;
    //name of the city
    public TMP_Text cityText;
    //visualization objects
    //Error
    public GameObject errorVisualization;
    //SUN
    public GameObject sunVisualization;
    //RAIN
    public GameObject rainVisualization;
    //SNOW
    public GameObject snowVisualization;
    //CLOUDS
    public GameObject cloudsVisualization;
    //THUNDERSTORN  
    public GameObject thunderstormVisualization;

    // Start is called before the first frame update
    void Start()
    {
        //empty start
        disableAllVisualizations();
    }

    // Update is called once per frame
    void Update()
    {
        //get the weather
        WeatherGetter.Result<WeatherResult>? weather = weatherGetter.getWeather();
        if (weather is WeatherGetter.Result<WeatherResult> weatherRes) {
            if(!weatherRes.isOk){
                cityText.text = "Fehler ...";
                //this means the date is outside of the allowed range
                if (weatherGetter.errorString == "HTTP/1.1 400 Bad Request")
                {
                    cityText.text = "invalid date";
                }
                temperatureText.text = "";
                friendlyNameText.text = "";
                activateVisualizationObject(null);
            }
            //activate the right visualization
            else{
                activateVisualizationObject(weatherRes.value.weatherType);
                temperatureText.text = weatherRes.value.temp.ToString("0.0") + " °C";
                friendlyNameText.text = weatherToFriendlyString(weatherRes.value.weatherType);
                cityText.text = weatherRes.value.city;
            }
        }
        else{
            temperatureText.text = "";
            friendlyNameText.text = "";
            cityText.text = "Laden ...";
        }

    }

    //now with english strings
    string weatherToFriendlyString(WeatherResult.WeatherType weather){
        switch(weather){
            case WeatherResult.WeatherType.RAIN:
                return "rain";
            case WeatherResult.WeatherType.SUN:
                return "sun";
            case WeatherResult.WeatherType.SNOW:
                return "snow";
            case WeatherResult.WeatherType.CLOUDS:
                return "clouds";
            case WeatherResult.WeatherType.THUNDERSTORM:
                return "thunderstorm";
            //in case of new types of weather, add them here, eg. 
            default:
                return "Todo: Implement";
        }

    }

    void disableAllVisualizations()
    {
        errorVisualization.SetActive(false);
        sunVisualization.SetActive(false);
        snowVisualization.SetActive(false);
        cloudsVisualization.SetActive(false);
        rainVisualization.SetActive(false);
        thunderstormVisualization.SetActive(false);
    }

    //bad code: emergency fix
    void activateVisualizationObject(WeatherResult.WeatherType? weather)
    {
        if (weather is null)
        {
            errorVisualization.SetActive(true);
            sunVisualization.SetActive(false);
            snowVisualization.SetActive(false);
            cloudsVisualization.SetActive(false);
            rainVisualization.SetActive(false);
            thunderstormVisualization.SetActive(false);
            return;
        }
        switch (weather)
        {
            case WeatherResult.WeatherType.RAIN:
                errorVisualization.SetActive(false);
                sunVisualization.SetActive(false);
                snowVisualization.SetActive(false);
                cloudsVisualization.SetActive(false);
                thunderstormVisualization.SetActive(false);
                rainVisualization.SetActive(true);
                return;
            case WeatherResult.WeatherType.SUN:
                errorVisualization.SetActive(false);
                snowVisualization.SetActive(false);
                cloudsVisualization.SetActive(false);
                rainVisualization.SetActive(false);
                thunderstormVisualization.SetActive(false);
                sunVisualization.SetActive(true);
                return;
            case WeatherResult.WeatherType.SNOW:
                errorVisualization.SetActive(false);
                sunVisualization.SetActive(false);
                cloudsVisualization.SetActive(false);
                rainVisualization.SetActive(false);
                thunderstormVisualization.SetActive(false);
                snowVisualization.SetActive(true);
                return;
            case WeatherResult.WeatherType.CLOUDS:
                errorVisualization.SetActive(false);
                sunVisualization.SetActive(false);
                snowVisualization.SetActive(false);
                rainVisualization.SetActive(false);
                thunderstormVisualization.SetActive(false);
                cloudsVisualization.SetActive(true);
                return;
            case WeatherResult.WeatherType.THUNDERSTORM:
                errorVisualization.SetActive(false);
                sunVisualization.SetActive(false);
                snowVisualization.SetActive(false);
                cloudsVisualization.SetActive(false);
                rainVisualization.SetActive(false);
                thunderstormVisualization.SetActive(true);
                return;
            default:
                Debug.Log("todo: implement");
                return;
        }
    }
}