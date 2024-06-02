using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class currentWeather {

    //Default Data
    public double temp = 0.0f;
    public int id = 0;
    public string main = "default";
    public string description = "default";
    public string icon = "###";

    public currentWeather(double temp, int id, string main, string description, string icon) {

        this.temp = temp;
        this.id = id;
        this.main = main;
        this.description = description;
        this.icon = icon;

    }

}
    
