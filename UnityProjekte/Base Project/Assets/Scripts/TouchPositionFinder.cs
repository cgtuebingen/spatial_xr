using System;
using UnityEngine;


public class TouchPositionFinder : MonoBehaviour
{
    public float correction = 155.0f;
    public WeatherGetter weatherGetter;
    public OSMRequest zoomMap;
    public float coolDown = 0.0f;
    

    private void Update()
    {
        Vector3 test = new Vector3(1, 1, 1);
        Quaternion rot = gameObject.transform.rotation;
        test = Quaternion.Inverse(rot) * test;
        Debug.DrawLine(transform.position, transform.position + test, Color.red);

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
        gameObject.SetActive(false);
        zoomMap.gameObject.SetActive(true);
        if(coolDown + 1.0f > Time.time) return;
        coolDown = Time.time;
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        Vector3 touchVector = (collisionPoint - gameObject.transform.position).normalized;
        //compensate for rotation

        Debug.Log("Before:" + touchVector);
        //life is pain
        Vector3 angles = gameObject.transform.rotation.eulerAngles;
        Quaternion rot = gameObject.transform.rotation;
        Quaternion sphereRot = Quaternion.Euler(270,0,0);
        
        Debug.Log(rot.eulerAngles);
        //x-Axis
        //touchVector = rotateAround(90, touchVector, new Vector3(1,0,0));
        //y-Axis
        //touchVector = rotateAround(180, touchVector, new Vector3(0, 1, 0));
        //z-Axis
        
        touchVector = Quaternion.Inverse(rot) * touchVector;
        touchVector = sphereRot * touchVector;
        Debug.Log("inv ang" + Quaternion.Inverse(rot).eulerAngles);
        //touchVector = rotateAround(180, touchVector, Vector3.up);
        
        Debug.Log("After:" + touchVector);
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
        Debug.Log(lat + ", " + lon);
        zoomMap.UpdateTile(lat,lon);

    }

    public Vector3 rotateAround(float angle, Vector3 vector, Vector3 axis)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        return rotation * vector;
    }
}
