using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPositionFinder : MonoBehaviour
{
    public float correction = 155.0f;
    public WeatherGetter weatherGetter;
    private int lastTouch = 0;

    void OnCollisionEnter(Collision collision) {
            if(Time.frameCount - lastTouch < 180) return;
            Debug.Log(collision.contacts.Length);
            ContactPoint cord = collision.contacts[0];
            //Debug.Log("Contactpoint is " + cord);
            Vector3 touchVector = (cord.point - gameObject.transform.position).normalized;

            Debug.Log(touchVector);
            float x = touchVector.x;
            //why why why
            float y = touchVector.z;
            float z = touchVector.y;
            //mostly stolen from https://www.movable-type.co.uk/scripts/latlong-vectors.html
            float lat = Mathf.Atan2(z,Mathf.Sqrt(x*x+y*y));
            float lon = Mathf.Atan2(y,x);
            //from RAD to DEV
            lat = (lat/Mathf.PI)*180;
            lon = (lon/Mathf.PI)*180;
            lon+=correction;
            //modulo
            lon -= lon > 180?180:0;
            weatherGetter.updateLocation(lat,lon);
            lastTouch = Time.frameCount;
        //Only WORKS FOR WORLD (Simpleworld2)


        Destroy(GameObject.FindWithTag("World")); //We can change the name of SimpleWorld2 without needing to change the code
    }
    //todo: check
    /*
    void OnTriggerEnter(Collider obj){
            Vector3 touchVector = (obj.transform.position - gameObject.transform.position).normalized;

            Debug.Log(touchVector);
            float x = touchVector.x;
            //why why why
            float y = touchVector.z;
            float z = touchVector.y;
            //mostly stolen from https://www.movable-type.co.uk/scripts/latlong-vectors.html
            float lat = Mathf.Atan2(z,Mathf.Sqrt(x*x+y*y));
            float lon = Mathf.Atan2(y,x);
            //from RAD to DEV
            lat = (lat/Mathf.PI)*180;
            lon = (lon/Mathf.PI)*180;
            lon+=correction;
            //modulo
            lon -= lon > 180?180:0;
            weatherGetter.updateLocation(lat,lon);
    }*/
}
