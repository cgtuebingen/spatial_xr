using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
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
        textures = new Texture2DArray(256, 256, (int)Math.Pow(4, zoomLevel), TextureFormat.ARGB32, true);
        material = GetComponent<Renderer>().material;
        for (int x = 0; x < (int)Math.Pow(2, zoomLevel); x++)
        {
            for (int y = 0; y < (int)Math.Pow(2, zoomLevel); y++)
            {
                Debug.Log("" + x + y + zoomLevel);
                StartCoroutine(GetText(layer, x, y, zoomLevel));
            }
        }
        material.SetTexture("_TexList", textures);
        material.SetInteger("_ZoomLevel",zoomLevel);
    }
    IEnumerator GetText(string layer, int x, int y, int z)
    {
        if (false)
        {
            Color[] colors = new Color[256*256];
            Array.Fill(colors, Color.black);
            textures.SetPixels(colors,1,0);
            textures.Apply(true);

            yield break;
                
        }
        string url = baseURl + layer + "/"+z+"/"+x+"/"+y+".png?appid=" + apiKey;
        Debug.Log(url);
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
