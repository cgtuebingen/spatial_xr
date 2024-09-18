using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using TMPro;


public class weatherConsumer : MonoBehaviour {


    [SerializeField] private float lat = 42.2f;
    [SerializeField] private float lon = 12.0f;
    [SerializeField] public string timeString = "";
     public ClockController clockController; // Referenz auf ClockController.

    public DateTime time;
    public WeatherGetter weatherGetter;
    WeatherResult weather;
    bool printed = false;

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

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update() {

        // Time will be updated if the date on the clock has been changed.
        int Day = ConvertAndLogConcatenatedInt("first_day_text", "second_day_text");
        int Month = ConvertAndLogConcatenatedInt("first_month_text", "second_month_text");
       
        if (clockController.isTimeChangedManually == true) {
        time = new DateTime(2024, Month, Day);
         Debug.Log("time" + time);
    
        weatherGetter.setRequestTime(time);

        WeatherGetter.Result<WeatherResult>? weather = weatherGetter.getWeather();
        if (weather is WeatherGetter.Result<WeatherResult> weatherRes ) {
            if(!weatherRes.isOk) Debug.Log("Error");
            else if(!printed){
                Debug.Log("Weather from ID:" + weatherRes.value.weatherType);
                Debug.Log("Temp:" + weatherRes.value.temp);
                Debug.Log(weatherRes.value.city);
                printed = true;
                clockController.isTimeChangedManually = false;
            }
        }
        else Debug.Log("Waiting for request");

        }
        
    }

    int ConvertAndLogConcatenatedInt(string gameObjectName1, string gameObjectName2)
{
    try
    {
        string concatenatedText = "";

        // GameObject 1
        GameObject gameObject1 = GameObject.Find(gameObjectName1);
        if (gameObject1 == null)
        {
            throw new Exception("GameObject " + gameObjectName1 + " nicht gefunden.");
        }

        TextMeshProUGUI textComponent1 = gameObject1.GetComponent<TextMeshProUGUI>();
        if (textComponent1 == null)
        {
            throw new Exception("TextMeshProUGUI-Komponente nicht gefunden für " + gameObjectName1);
        }

        concatenatedText += textComponent1.text;

        // GameObject 2
        GameObject gameObject2 = GameObject.Find(gameObjectName2);
        if (gameObject2 == null)
        {
            throw new Exception("GameObject " + gameObjectName2 + " nicht gefunden.");
        }

        TextMeshProUGUI textComponent2 = gameObject2.GetComponent<TextMeshProUGUI>();
        if (textComponent2 == null)
        {
            throw new Exception("TextMeshProUGUI-Komponente nicht gefunden für " + gameObjectName2);
        }

        concatenatedText += textComponent2.text;

        // Conversion to Integer
        int intValue;
        if (!int.TryParse(concatenatedText, out intValue))
        {
            throw new Exception("Text konnte nicht zu Integer konvertiert werden: " + concatenatedText);
        }
    //     Debug.Log("int value  " + intValue);
        return intValue;
    }
    catch (Exception ex)
    {
      Debug.LogError("Fehler in ConvertAndLogConcatenatedInt: " + ex.Message);
        
        throw; 
    }
}

}

