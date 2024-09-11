using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInteractionController : MonoBehaviour
{
    public OSMRequest map;
    public int zoomEffectMagnitude;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //does not look good anymore
        if (map.zoom <= 2 || map.zoom >= 18) return;
        map.UpdateTile(map.lat,map.lon, map.zoom+zoomEffectMagnitude);
    }
}

