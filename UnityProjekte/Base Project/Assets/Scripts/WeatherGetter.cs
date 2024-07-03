using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class WeatherGetter : MonoBehaviour
{
    //API Key is hidden inside a static class
    private string apiKeyOWM = APIKey.apiKey;
    private string baseURLOWM = "https://api.openweathermap.org/data/2.5/weather?appid=";
    private string baseURLOPENM = "https://api.open-meteo.com/v1/forecast";
    //will be null unless something has been requested
    private Result<string>? result = null;
    //the time when the weather should be requested
    private DateTime requestTime;
    //the API the current request should be sent to
    private API api;
    void start()
    {
        requestTime = DateTime.Now;
    }
    IEnumerator GetRequest(string url)
    {
        //using (UnityWebRequest www = UnityWebRequest.Get(baseURL+"&appid=" + apiKey +"&lat=" + lat + "&lon=" + lon ))
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                result = Result<string>.Ok(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error: " + www.error);
                result = Result<string>.Error(www.error);
            }
        } // The using block ensures www.Dispose() is called when this block is exited

    }
    // this will return the current weather if something was requested. Otherwise it will continue return null, unless something arrives
    public Result<WeatherResult>? getWeather()
    {
        if (result == null) return null;
        Debug.Log(result.Value.value);
        if (!result.Value.isOk) return Result<WeatherResult>.Error(null);
    

        switch (api){
            case API.OPEN_WEATHER_MAP:
                OWMResult res = JsonUtility.FromJson<OWMResult>(result?.value);

                Weather[] wea = res.weather.ToArray();

                return Result<WeatherResult>.Ok(new OWMWeather(res.main.temp, wea[0].id, res.name));
            case API.OPEN_METEO:
                OpenMeteoResult resb = JsonUtility.FromJson<OpenMeteoResult>(result?.value);
                return Result<WeatherResult>.Ok( new OpenMeteoWeather(resb.hourly.temperature_2m[requestTime.Hour],resb.hourly.weather_code[requestTime.Hour]));
            default:
                return null;
            }

        

    }
    //this start a lookup for the weather at a new location, it invalidates the currently cached weather and makes no guarantee that something new will replace it (eg. the internet may be down)
    public void updateLocation(float lat, float lon)
    {
        //invalidate existing result, since the requested location has changed
        result = null;
        //start coroutine
        string requestString = "";
        //We want the weather now (approx)
        if (requestTime.Subtract(DateTime.Now).TotalHours <= 1)
        {
            requestString = baseURLOWM + apiKeyOWM + "&lat=" + lat + "&lon=" + lon;
            api = API.OPEN_WEATHER_MAP;
        }
        else
        {
            requestString = baseURLOPENM + "?latitude=" + lat + "&longitude=" + lon + "&hourly=temperature_2m,weather_code&start_date=" + requestTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "&end_date=" + requestTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            api = API.OPEN_METEO;
        }
        Debug.Log(requestString);
        Coroutine rout = StartCoroutine(GetRequest(requestString));
    }
    //same as updateLocation, but with a string instead of coordinates, expects sanitized input
    public void updateLocationFromString(string city)
    {
        //invalidate existing result, since the requested location has changed
        result = null;
        //start coroutine
        string requestString = "";
        //We want the weather now (approx), disable OpenMeteo for now, it does not support query by string
        if (requestTime.Subtract(DateTime.Now).TotalHours <= 1 || true)
        {
            requestString = baseURLOWM + apiKeyOWM + "&q=" + city;
            api = API.OPEN_WEATHER_MAP;
        }
        else
        {
            //todo handle this case
            requestString = "";
            api = API.OPEN_METEO;
        }
        Debug.Log(requestString);
        Coroutine rout = StartCoroutine(GetRequest(requestString));
        //start coroutine

    }
    public void setRequestTime(DateTime date)
    {
        requestTime = date;
    }
    enum API
    {
        OPEN_WEATHER_MAP, OPEN_METEO
    }
    public struct Result<T>{
        public static Result<T> Error(T value) => new Result<T>(value,false);
        public static Result<T> Ok(T value) => new Result<T>(value,true);
        public bool isOk;
        public T value{get;}
        Result(T value, bool isOk)
        {
           this.value = value;
            this.isOk = isOk;
        }


    }

}