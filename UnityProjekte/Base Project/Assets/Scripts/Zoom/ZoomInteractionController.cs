using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInteractionController : MonoBehaviour
{
    public OSMRequest map;
    public int zoomEffectVal;

    private float coolDown = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(coolDown + 1f > Time.time) return;
        coolDown = Time.time;
        //does not look good anymore
        if (map.zoom <= 3 || map.zoom >= 18) return;
        map.changeZoom(zoomEffectVal);
    }
}

