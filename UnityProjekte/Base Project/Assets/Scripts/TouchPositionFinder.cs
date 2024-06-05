using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPositionFinder : MonoBehaviour
{
    //This Script will print out the coordinates of the touch point, if the "touchPoint" comes closer that touchDistance to the sphere (earth). This is just an example how these coordinates can be used.
    //IMPORTANT: The sphere (globe) should be oriented so that north is up (Unity:y) and the prime meridian points in x direction. The current sphere is oriented correctly.
    
    public float radiusOfSphere;
    //all touch things will be referenced to this point
    public GameObject touchPoint;
    //how far from the sphere, so it still registers as a touch
    public double touchDistance;
    //rename for clarity
    GameObject center;


    // Start is called before the first frame update
    void Start()
    {
        //GameObject with this script
        center = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float touchToCenterDistance = Vector3.Distance(center.transform.position,touchPoint.transform.position);
        //we touch the sphere (with some margin)
        if(Mathf.Abs(touchToCenterDistance-radiusOfSphere)<=touchDistance){
            Vector3 touchVector = (touchPoint.transform.position - center.transform.position).normalized;
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
            Debug.Log(lat);
            Debug.Log(lon);
        }
        
    }

}
