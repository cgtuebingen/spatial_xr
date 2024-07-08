using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class OSMRequest : MonoBehaviour
{
    public RawImage mapImage;
    public float lat; // Breitengrad
    public float lon; // Längengrad
    public int zoom = 1;
    private string urlTemplate = "https://tile.openstreetmap.org/{z}/{x}/{y}.png";

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Initialize min and max vectors
        Vector3 min = mesh.vertices[0];
        Vector3 max = mesh.vertices[0];

        // Iterate through all vertices to find the min and max
        foreach (Vector3 vertex in mesh.vertices)
        {
            if (vertex.x < min.x) min.x = vertex.x;
            if (vertex.y < min.y) min.y = vertex.y;
            if (vertex.z < min.z) min.z = vertex.z;

            if (vertex.x > max.x) max.x = vertex.x;
            if (vertex.y > max.y) max.y = vertex.y;
            if (vertex.z > max.z) max.z = vertex.z;
        }

        // Print the results
        Debug.Log("Min Vertex: " + min);
        Debug.Log("Max Vertex: " + max);
        
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex",Texture2D.blackTexture);
        
        StartCoroutine(LoadTile());
    }

    IEnumerator LoadTile()
    {
        // Umrechnung der geografischen Koordinaten in Kachelkoordinaten
        int tileX, tileY;
        GeoToTile(lat, lon, zoom, out tileX, out tileY);
        gameObject.GetComponent<Renderer>().material.SetFloat("_ZoomLevel",Mathf.Pow(2,zoom));

        // Ersetze Platzhalter in der URL mit den tatsächlichen Werten
        string url = urlTemplate.Replace("{z}", zoom.ToString())
                                .Replace("{x}", tileX.ToString())
                                .Replace("{y}", tileY.ToString());

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading tile: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                //mapImage.texture = texture; // Setze die geladene Textur als Bild
                gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex",texture);
            }
        }
    }

void GeoToTile(double lat, double lon, int zoom, out int tileX, out int tileY)
{
    double latRad = lat * Math.PI / 180.0;
    tileX = (int)((lon + 180.0) / 360.0 * (1 << zoom));
    tileY = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * (1 << zoom));
}

    public void UpdateTile(float newLat, float newLon, int newZoom = 14) 
    {
        lat = newLat;
        lon = newLon;
        zoom = newZoom;
        OnCollisionEnter(null);
        StartCoroutine(LoadTile());
    }

    public void OnCollisionEnter(Collision other)
    {
        GeoToTile(lat,lon,zoom,out int x, out int y);
        
        Vector3 corner = gameObject.transform.position - new Vector3(5, 0, 0);
        float lonBottomLeft = tileToLon(x, zoom);
        float latBottomLeft = tileToLat(y + 1, zoom);
        float lonTopRight = tileToLon(x + 1, zoom);
        float latTopRight = tileToLat(y, zoom);
        Vector3 touch = new Vector3(10, 0, 1);
        float latStep = latTopRight - latBottomLeft;
        float lonStep = lonTopRight - lonBottomLeft;
        float resLat = (touch.x / 10.0f) * latStep + latBottomLeft;
        float resLon = (touch.z / 10.0f) * lonStep + lonBottomLeft;
        Debug.Log(resLat + ", "+ resLon);
    }
    private float tileToLon(float x, float zoom){
        float lonBottomLeft = (x / Mathf.Pow(2, zoom)) * 360f - 180f;
        return lonBottomLeft;
    }

    private float tileToLat(float y, float zoom)
    {
        float latBottomLeft = Mathf.PI * (1 - 2 * y / Mathf.Pow(2, zoom));
        latBottomLeft = (Mathf.Atan((float)Math.Sinh(latBottomLeft))/(2*Mathf.PI))*360;
        return latBottomLeft;
    }
    
}