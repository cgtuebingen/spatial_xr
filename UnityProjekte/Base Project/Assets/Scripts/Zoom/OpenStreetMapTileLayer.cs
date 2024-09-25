using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
//Request to OpenStreetMap
public class OSMRequest : MonoBehaviour
{
    public float lat; // Breitengrad
    public float lon; // Längengrad
    public int zoom = 1;
    private string urlTemplate = "https://tile.openstreetmap.org/{z}/{x}/{y}.png";
    private float coolDown = 0.0f;
    public WeatherGetter WeatherGetter;
    private Vector3 firstPos;
    private Texture2DArray textures;
    private float offsetX = 0.0f;
    private float offsetY = 0.0f;
    private Material mainMat;
    public float swipeSensitivity = 0.1f;
    private Vector2 velocity;
    public float swipeVelocity = 0.2f;
    private TileCache cache;
    private float zoomOffset = 1.0f;
    //tracks the time between entry and exit of finger (for velocity)
    private float delta = 0.0f;
    void Start()
    {
        mainMat = gameObject.GetComponent<Renderer>().material;
        textures = new Texture2DArray(256, 256, 9, TextureFormat.ARGB32, true);
        mainMat.SetTexture("_MainTex", Texture2D.blackTexture);
        mainMat.SetTexture("_TexList", textures);
        velocity = new Vector2(0f, 0f);
        //initialize the cache
        cache = new TileCache();
        //initialize, then wait for click
        gameObject.SetActive(false);
    }

    private void Update()
    {
        //apply velocity
        offsetX += velocity.x;
        offsetY += velocity.y;
        //apply zoom, gradually slow down zooom by mean
        zoomOffset = (zoomOffset + 0.1f)/1.1f;
        //clamp
        offsetX = Mathf.Clamp(offsetX, -1, 1);
        offsetY = Mathf.Clamp(offsetY, -1, 1);
        //send to gpu
        mainMat.SetFloat("_OffsetX",offsetX);
        mainMat.SetFloat("_OffsetY",offsetY);
        mainMat.SetFloat("_ZoomOffset", zoomOffset);
        //slow down velocity
        velocity = velocity * 0.95f;
    }
    //Load a specific tile into a slot in the texture list
    IEnumerator LoadTile(int offsetTileIndex)
    {
        // Umrechnung der geografischen Koordinaten in Kachelkoordinaten
        int tileX, tileY;
        GeoToTile(lat, lon, zoom, out tileX, out tileY);
        mainMat.SetFloat("_ZoomLevel",Mathf.Pow(2,zoom));
        int offsetX = (offsetTileIndex % 3)-1;
        int offsetY = (offsetTileIndex / 3)-1;
        tileX += offsetX;
        tileY -= offsetY;
        if (tileX < 0)
        {
            tileX += (int)Math.Pow(2, zoom);
        }

        if (tileY < 0)
        {
            tileY += (int)Math.Pow(2, zoom);
        }
        //check the cache first!!
        Color[] res = cache.Get(tileX, tileY, zoom);
        if (res != null)
        {
            textures.SetPixels(res, offsetTileIndex);
            textures.Apply(true);
            yield break;
        }
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
                for (int i = 0; i < 9; i++)
                {
                    //Color[] colors = new Color[256*256];
                    //Array.Fill(colors, Color.black);
                    //arr.SetPixels(colors,i,0);
                    
                }
                //insert into the Cache
                cache.Insert(tileX,tileY,zoom,texture.GetPixels());
                textures.SetPixels(texture.GetPixels(), offsetTileIndex);
                textures.Apply(true);
                //mainMat.SetTexture("_MainTex",texture);
            }
        }
    }
//convert coordinates to tile
    void GeoToTile(double lat, double lon, int zoom, out int tileX, out int tileY)
    {
        double latRad = lat * Math.PI / 180.0;
        tileX = (int)((lon + 180.0) / 360.0 * (1 << zoom));
        tileY = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * (1 << zoom));
    }
    //change new center
    public void UpdateTile(float newLat, float newLon, int newZoom = 7) 
    {
        //reset offsets
        offsetX = 0;
        offsetY = 0;
        lat = newLat;
        lon = newLon;
        zoom = newZoom;
        for (int i = 0; i < 9; i++)
        {
            StartCoroutine(LoadTile(i));
        }
    }

    //Set first point
    private void OnTriggerEnter(Collider other)
    {
        delta = Time.time;
        Vector3 collisionPoint = transform.InverseTransformPoint(other.transform.position);
        firstPos = collisionPoint;
    }
    //react to swipe/poke
    private void OnTriggerExit(Collider other)
    {
        if(coolDown + 0.3f > Time.time) return;
        coolDown = Time.time;
        Vector3 collisionPoint = transform.InverseTransformPoint(other.transform.position);
        float deltaTime = (Time.time - delta);
        //swipe detection, a swipe covers a long distance
        Vector3 contactVector = firstPos - collisionPoint;
        //the swipe sensitivity seems to be dependent on the device used. Why? I don't know
        float swipeSens = 0;
        //the collider was a finger
        if(other.name.Equals("Capsule Collider Proximal"))
        {
            swipeSens = 1f;

        }
        //controller    
        else
        {
            swipeSens = 2f;
        }
        //poke
        if (contactVector.magnitude <= swipeSens)
        {
            //get the world coordinates of the four corner points
            GeoToTile(lat, lon, zoom, out int x, out int y);
            float lonBottomLeft = tileToLon(x, zoom);
            float latBottomLeft = tileToLat(y + 1, zoom);
            float lonTopRight = tileToLon(x + 1, zoom);
            float latTopRight = tileToLat(y, zoom);
            //get touch as vector
            Vector3 corner =  new Vector3(-5f, -0f, -5f);
            //account for rotation gameObject.transform.position -
            Vector3 touch = new Vector3(10,0,10) - (collisionPoint - corner);
            float latStep = latTopRight - latBottomLeft;
            float lonStep = lonTopRight - lonBottomLeft;
            //linear interpolation between the corners, dependent on the touch vector
            float resLat = ((touch.z / 10f) + offsetY) * latStep + latBottomLeft;
            float resLon = ((touch.x / 10f) + offsetX) * lonStep + lonBottomLeft;
            //send to weatherGetter
            WeatherGetter.updateLocation(resLat, resLon);
            //hide on click
            gameObject.SetActive(false);

            
        }
        //swipe
        else
        {
            velocity =  new Vector2(contactVector.x, contactVector.z) * -swipeVelocity/deltaTime;
        }
        
    }
    //zoom in or out
    public void changeZoom(int step)
    {
        if (step == 1)
        {
            zoomOffset = 2f;
        }
        else zoomOffset = 0.5f;
        //recenter, same as in OnTriggerExit
        GeoToTile(lat, lon, zoom, out int x, out int y);
        float lonBottomLeft = tileToLon(x, zoom);
        float latBottomLeft = tileToLat(y + 1, zoom);
        float lonTopRight = tileToLon(x + 1, zoom);
        float latTopRight = tileToLat(y, zoom);
        float latStep = latTopRight - latBottomLeft;
        float lonStep = lonTopRight - lonBottomLeft;
        float resLat = (offsetY+0.5f) * latStep + latBottomLeft;
        float resLon = (offsetX+0.5f) * lonStep + lonBottomLeft;
        offsetX = 0;
        offsetY = 0;
        UpdateTile(resLat, resLon, zoom + step);
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