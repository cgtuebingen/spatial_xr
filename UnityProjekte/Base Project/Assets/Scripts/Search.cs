using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//wtf
using UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard;
public class Search : MonoBehaviour
{
    public WeatherGetter weatherGetter;

    //this gets run on "enter"
    public void submit(){
        weatherGetter.updateLocationFromString(GlobalNonNativeKeyboard.instance.keyboard.text);
    }
    //show the keyboard on "click"
    void OnTriggerEnter(){
        GlobalNonNativeKeyboard.instance.ShowKeyboard();
    }
}
