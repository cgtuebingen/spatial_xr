using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPositionFinder : MonoBehaviour
{
    public float correction = 155.0f;
    public WeatherGetter weatherGetter;
    public OSMRequest zoomMap;
    public float coolDown = 0.0f;
    
    private void Start()
    {
        
        //zoomMap.UpdateTile(48.523083f,9.060778f);
        zoomMap.gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision) {
        return;
            Debug.Log("Collision");
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
            zoomMap.UpdateTile(lat,lon);
            //weatherGetter.updateLocation(lat,lon);
        //Only WORKS FOR WORLD (Simpleworld2)


        Destroy(GameObject.FindWithTag("World")); //We can change the name of SimpleWorld2 without needing to change the code
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("asdf");
        Debug.Log(other.GetComponent<Rigidbody>().velocity*10000000000.0f);
        zoomMap.gameObject.SetActive(true);
        if(coolDown + 1.0f > Time.time) return;
        coolDown = Time.time;
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        Vector3 touchVector = (collisionPoint - gameObject.transform.position).normalized;
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
        zoomMap.UpdateTile(lat,lon);
        gameObject.SetActive(false);
    }
}
