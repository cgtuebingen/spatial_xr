using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
//using Unity.VisualScripting;

public class WeatherGetter : MonoBehaviour
{
    //API Key is hidden inside a static class
    private string apiKey = APIKey.apiKey;
    private string baseURL = "https://api.openweathermap.org/data/2.5/weather?appid=";
   //will be null unless something has been requested
    private string result = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GetRequestCordinates(float lat, float lon)
{
    //using (UnityWebRequest www = UnityWebRequest.Get(baseURL+"&appid=" + apiKey +"&lat=" + lat + "&lon=" + lon ))
    using (UnityWebRequest www = UnityWebRequest.Get(baseURL + apiKey + "&lat=" + lat + "&lon=" + lon ))
    {
        
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            result = www.downloadHandler.text;
            Root res = JsonUtility.FromJson<Root>(result);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    } // The using block ensures www.Dispose() is called when this block is exited

}
    IEnumerator GetRequestString(string searchstring)
{
    //using (UnityWebRequest www = UnityWebRequest.Get(baseURL+"&appid=" + apiKey +"&lat=" + lat + "&lon=" + lon ))
    using (UnityWebRequest www = UnityWebRequest.Get(baseURL + apiKey + "&q=" + searchstring))
    {
        
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            result = www.downloadHandler.text;
            Root res = JsonUtility.FromJson<Root>(result);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    } // The using block ensures www.Dispose() is called when this block is exited

}
// this will return the current weather if something was requested. Otherwise it will continue return null, unless something arrives
public currentWeather getWeather(){
        if (result != null) {
            Root res = JsonUtility.FromJson<Root>(result);

            Weather[] wea = res.weather.ToArray();

            return new currentWeather(res.main.temp, wea[0].id, res.name);
    }
    else return null;

}
//this start a lookup for the weather at a new location, it invalidates the currently cached weather and makes no guarantee that something new will replace it (eg. the internet may be down)
public void updateLocation(float lat, float lon){
    //invalidate existing result, since the requested location has changed
    result = null;
    //start coroutine
    Coroutine rout = StartCoroutine(GetRequestCordinates(lat,lon));
}
//same as updateLocation, but with a string instead of coordinates, expects sanitized input
public void updateLocationFromString(string city){
    //invalidate existing result, since the requested location has changed
    result = null;
    //start coroutine
    Coroutine rout = StartCoroutine(GetRequestString(city));

}
}