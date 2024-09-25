using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//this loads the requested tile from OpenStreetMap
public class WeatherMapGetter : MonoBehaviour
{
    private Material material;
    private string apiKey = APIKey.apiKey;
    public string layer = "clouds_new";
    private string baseURl = "https://tile.openweathermap.org/map/";
    public int zoomLevel = 1;
    private Texture2DArray textures;
    void Start()
    {
        //initialize textures and material
        textures = new Texture2DArray(256, 256, (int)Math.Pow(4, zoomLevel), TextureFormat.ARGB32, true);
        material = GetComponent<Renderer>().material;
        for (int x = 0; x < (int)Math.Pow(2, zoomLevel); x++)
        {
            for (int y = 0; y < (int)Math.Pow(2, zoomLevel); y++)
            {
                StartCoroutine(GetText(layer, x, y, zoomLevel));
            }
        }
        //write attributes to shader
        material.SetTexture("_TexList", textures);
        material.SetInteger("_ZoomLevel",zoomLevel);
    }
    //request tile from OSM
    IEnumerator GetText(string layer, int x, int y, int z)
    {
        string url = baseURl + layer + "/"+z+"/"+x+"/"+y+".png?appid=" + apiKey;
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                textures.SetPixels(DownloadHandlerTexture.GetContent(uwr).GetPixels(),y*(int)Math.Pow(2,zoomLevel)+x);
                //textures.SetPixels(DownloadHandlerTexture.GetContent(uwr).GetPixels(),0,0);
                textures.Apply(true);
                material.SetTexture("_MainTexture",DownloadHandlerTexture.GetContent(uwr));
                //material.SetTexture("_TexList",textures);
                //material.mainTexture = DownloadHandlerTexture.GetContent(uwr);
            }
        }
    }


}
