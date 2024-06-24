using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class WeatherMapGetter : MonoBehaviour
{
    private Material material;
    // Start is called before the first frame update
    private string apiKey = APIKey.apiKey;
    public string layer;
    private string baseURl = "https://tile.openweathermap.org/map/";
    void Start()
    {
        material = GetComponent<Renderer>().material;
        material.mainTexture = Texture2D.blackTexture;
        StartCoroutine(GetText(layer, 0, 0, 0));
    }

    IEnumerator GetText(string layer, int x, int y, int z)
    {
        string url = baseURl + layer + "/"+x+"/"+y+"/"+z+".png?appid=" + apiKey;
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
                Debug.Log(DownloadHandlerTexture.GetContent(uwr).format);
                material.mainTexture = DownloadHandlerTexture.GetContent(uwr);
            }
        }
    }


}
