using UnityEngine;

public class TouchPositionFinder : MonoBehaviour
{
    public float correction = 180.0f;
    public WeatherGetter weatherGetter;
    public OSMRequest zoomMap;
    public float coolDown = 0.0f;
    private void OnTriggerEnter(Collider other)
    {
        zoomMap.gameObject.SetActive(true);
        if(coolDown + 1.0f > Time.time) return;
        coolDown = Time.time;
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        Vector3 touchVector = (collisionPoint - gameObject.transform.position).normalized;
        //compensate for rotation
        //life is pain
        Quaternion rot = gameObject.transform.rotation;
        //so the earth faces the right way
        Quaternion sphereRot = Quaternion.Euler(270,0,0);
        //just turn back
        touchVector = Quaternion.Inverse(rot) * touchVector;
        touchVector = sphereRot * touchVector;

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

    }
}
