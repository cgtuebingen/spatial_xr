using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
//using Unity.VisualScripting;

public class WeatherGetter : MonoBehaviour
{
    private string apiKey = "";
    private string baseURL = "https://api.openweathermap.org/data/2.5/weather?appid=";
   [SerializeField] private float lat = 42.2f;
   [SerializeField] private float lon = 12.0f;
    private string result = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Coroutine rout =   StartCoroutine(GetRequest(lat,lon));
        
    }

    // Update is called once per frame
    void Update()
    {

        if(result != null) {
        Root res = JsonUtility.FromJson<Root>(result);
        }
    }
    IEnumerator GetRequest(float lat, float lon)
{
    //using (UnityWebRequest www = UnityWebRequest.Get(baseURL+"&appid=" + apiKey +"&lat=" + lat + "&lon=" + lon ))
    using (UnityWebRequest www = UnityWebRequest.Get(baseURL + apiKey + "&lat=" + lat + "&lon=" + lon ))
    {
        Debug.Log(baseURL+"&lat=" + lat + "&lon=" + lon + "&appid=" + apiKey);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + www.downloadHandler.text);
            result = www.downloadHandler.text;
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    } // The using block ensures www.Dispose() is called when this block is exited

}
public currentWeather getCurrentWeather(){
        if (result != null) {
            Root res = JsonUtility.FromJson<Root>(result);
            // Weather wea = JsonUtility.FromJson<Weather>(result);

            Weather[] wea = res.weather.ToArray();

            Debug.Log("Current Weather is updated");
            return new currentWeather(res.main.temp, wea[0].id, wea[0].main, wea[0].description, wea[0].icon);
    }
    else return null;

}
}
