using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Threading;
using TMPro;

public class weatherConsumer : MonoBehaviour
{
    [SerializeField] private float lat = 42.2f;
    [SerializeField] private float lon = 12.0f;
    [SerializeField] public string timeString = "";
    public ClockController clockController; // Referenz auf ClockController.

    public DateTime time;
    public WeatherGetter weatherGetter;
    WeatherResult weather;
    bool printed = false;

    int year;
    int month;
    int day;
    int hour;

    // In der Start-Methode die Initialisierung durchführen
    void Start()
    {
        // Initialisiere die Variablen erst im Start oder Update
        if (clockController != null)
        {
            year = int.Parse(clockController.yearTextSecond.text);
            month = int.Parse(clockController.monthTextSecond.text);
            day = int.Parse(clockController.dayTextSecond.text);
            hour = int.Parse(clockController.hourTextSecond.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Time will be updated if the date on the clock has been changed.
        if (clockController.isTimeChangedManually == true)
        {
            // Aktualisiere die Werte für Jahr, Monat, Tag, Stunde, wenn die Zeit manuell geändert wurde
            year = int.Parse(clockController.yearTextSecond.text);
            month = int.Parse(clockController.monthTextSecond.text);
            day = int.Parse(clockController.dayTextSecond.text);
            hour = int.Parse(clockController.hourTextSecond.text);

            // Erstelle ein neues DateTime-Objekt mit den aktualisierten Werten
            time = new DateTime(year, month, day, hour, 0, 0); // Stunden, Minuten, Sekunden

            Debug.Log("time: " + time);

            weatherGetter.setRequestTime(time);
            clockController.isTimeChangedManually = false;

            WeatherGetter.Result<WeatherResult>? weather = weatherGetter.getWeather();
            if (weather is WeatherGetter.Result<WeatherResult> weatherRes)
            {
                if (!weatherRes.isOk)
                    Debug.Log("Error");
                else if (!printed)
                {
                    Debug.Log("Weather from ID:" + weatherRes.value.weatherType);
                    Debug.Log("Temp:" + weatherRes.value.temp);
                    Debug.Log(weatherRes.value.city);
                    printed = true;
                }
            }
        }
    }
}
