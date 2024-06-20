using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//wtf
using UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard;
public class Search : MonoBehaviour
{
    public WeatherGetter weatherGetter;

    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void submit(){
        weatherGetter.updateLocationFromString(GlobalNonNativeKeyboard.instance.keyboard.text);
        Debug.Log(GlobalNonNativeKeyboard.instance.keyboard.text);
    }
    void OnTriggerEnter(){
        GlobalNonNativeKeyboard.instance.ShowKeyboard();
    }
}
