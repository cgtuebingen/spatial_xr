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
    //save the coordinates for update
    private Coords coords = null;
    private string city = "";
    public string errorString = "";
    void Start()
    {
        requestTime = DateTime.Now;
        requestTime = DateTime.Parse("2024-06-28",  null, System.Globalization.DateTimeStyles.RoundtripKind);
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
                errorString = www.error;
                result = Result<string>.Error(www.error);
            }
        } // The using block ensures www.Dispose() is called when this block is exited

    }
    //get request to openmeteo via a city name
    IEnumerator GetRequestToOMByCity(string city)
    {
        string url =  baseURLOWM + apiKeyOWM + "&q=" + city;
        //using (UnityWebRequest www = UnityWebRequest.Get(baseURL+"&appid=" + apiKey +"&lat=" + lat + "&lon=" + lon ))
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                OWMResult res = JsonUtility.FromJson<OWMResult>(www.downloadHandler.text);
                this.coords = new Coords(res.coord.lat,res.coord.lon);
                this.city = res.name;
                updateLocation(res.coord.lat, res.coord.lon);
            }
            else
            {
                errorString = www.error;
                Debug.Log("Error: " + www.error);
                result = Result<string>.Error(www.error);
            }
        } // The using block ensures www.Dispose() is called when this block is exited

    }
    IEnumerator GetRequestToOMBCoords(float lat, float lon)
    {
        string url = baseURLOWM + apiKeyOWM + "&lat=" + lat.ToString("0.000000", CultureInfo.InvariantCulture) + "&lon=" + lon.ToString("0.000000", CultureInfo.InvariantCulture);
        //using (UnityWebRequest www = UnityWebRequest.Get(baseURL+"&appid=" + apiKey +"&lat=" + lat + "&lon=" + lon ))
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                OWMResult res = JsonUtility.FromJson<OWMResult>(www.downloadHandler.text);
                this.city = res.name;
                url = baseURLOPENM + "?latitude=" + lat.ToString("0.000000", CultureInfo.InvariantCulture) + "&longitude=" + lon.ToString("0.000000", CultureInfo.InvariantCulture) + "&hourly=temperature_2m,weather_code&start_date=" + requestTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "&end_date=" + requestTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                StartCoroutine(GetRequest(url));
            }
            else
            {
                errorString = www.error;
                Debug.Log("Error: " + www.error);
                result = Result<string>.Error(www.error);
            }
        } // The using block ensures www.Dispose() is called when this block is exited

    }
    // this will return the current weather if something was requested. Otherwise it will continue return null, unless something arrives
    public Result<WeatherResult>? getWeather()
    {
        if (result == null) return null;
        //Debug.Log(result.Value.value);
        if (!result.Value.isOk) return Result<WeatherResult>.Error(null);
    

        switch (api){
            case API.OPEN_WEATHER_MAP:
                OWMResult res = JsonUtility.FromJson<OWMResult>(result?.value);

                Weather[] wea = res.weather.ToArray();

                return Result<WeatherResult>.Ok(new OWMWeather(res.main.temp, wea[0].id, res.name));
            case API.OPEN_METEO:
                OpenMeteoResult resb = JsonUtility.FromJson<OpenMeteoResult>(result?.value);
                return Result<WeatherResult>.Ok( new OpenMeteoWeather(resb.hourly.temperature_2m[requestTime.Hour],resb.hourly.weather_code[requestTime.Hour],city));
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
        this.coords = new Coords(lat,lon);
        //We want the weather now (approx)
        if (Math.Abs(requestTime.Subtract(DateTime.Now).TotalHours) <= 1)
        {
            requestString = baseURLOWM + apiKeyOWM + "&lat=" + lat.ToString("0.000000", CultureInfo.InvariantCulture) + "&lon=" + lon.ToString("0.000000", CultureInfo.InvariantCulture);
            api = API.OPEN_WEATHER_MAP;
            Coroutine rout = StartCoroutine(GetRequest(requestString));
        }
        else
        {

            //requestString = baseURLOPENM + "?latitude=" + lat + "&longitude=" + lon + "&hourly=temperature_2m,weather_code&start_date=" + requestTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "&end_date=" + requestTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            api = API.OPEN_METEO;
            StartCoroutine(GetRequestToOMBCoords(lat,lon));
        }
        //Debug.Log(requestString);
    }
    //same as updateLocation, but with a string instead of coordinates, expects sanitized input
    public void updateLocationFromString(string city)
    {
        //invalidate existing result, since the requested location has changed
        result = null;
        //start coroutine
        string requestString = "";
        //We want the weather now (approx), disable OpenMeteo for now, it does not support query by string
        if (requestTime.Subtract(DateTime.Now).TotalHours <= 1)
        {
            requestString = baseURLOWM + apiKeyOWM + "&q=" + city;
            api = API.OPEN_WEATHER_MAP;
            Coroutine rout = StartCoroutine(GetRequest(requestString));
            //Debug.Log(requestString);
        }
        else
        {
            Debug.Log("forecast");
            Coroutine rout = StartCoroutine(GetRequestToOMByCity(city));
        }



    }
    public void update(){
        if(coords != null) updateLocation(coords.lat,coords.lon);
    }
    public void setRequestTime(DateTime date)
    {
        requestTime = date;
        update();
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
    class Coords{
        public float lat;
        public float lon;
        public Coords(float lat, float lon){
            this.lat=lat;
            this.lon=lon;
        }
    }

}