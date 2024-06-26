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
    public int zoom = 14;
    private string urlTemplate = "https://tile.openstreetmap.org/{z}/{x}/{y}.png";

    void Start()
    {
        StartCoroutine(LoadTile());
    }

    IEnumerator LoadTile()
    {
        // Umrechnung der geografischen Koordinaten in Kachelkoordinaten
        int tileX, tileY;
        GeoToTile(lat, lon, zoom, out tileX, out tileY);

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
                mapImage.texture = texture; // Setze die geladene Textur als Bild
            }
        }
    }

void GeoToTile(double lat, double lon, int zoom, out int tileX, out int tileY)
{
    double latRad = lat * Math.PI / 180.0;
    tileX = (int)((lon + 180.0) / 360.0 * (1 << zoom));
    tileY = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * (1 << zoom));
}

    public void UpdateTile(float newLat, float newLon, int newZoom)
    {
        lat = newLat;
        lon = newLon;
        zoom = newZoom;
        StartCoroutine(LoadTile());
    }
}
