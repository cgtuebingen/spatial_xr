using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
    public class Hourly
    {
        public List<string> time;
        public List<double> temperature_2m;
        public List<int> weather_code;
    }
[System.Serializable]
    public class HourlyUnits
    {
        public string time;
        public string temperature_2m;
        public string weather_code;
    }
[System.Serializable]
    public class OpenMeteoResult
    {
        public double latitude;
        public double longitude;
        public double generationtime_ms;
        public int utc_offset_seconds;
        public string timezone;
        public string timezone_abbreviation;
        public double elevation;
        public HourlyUnits hourly_units;
        public Hourly hourly;
    }
