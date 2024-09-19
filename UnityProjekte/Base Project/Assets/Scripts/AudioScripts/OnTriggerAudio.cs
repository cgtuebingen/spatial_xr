using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnTriggerAudio : MonoBehaviour
{
    //Needs an audio Source with a file attached:
    public AudioSource triggerSound;
    
    /*
     *  @param Collider (with Trigger)
     *  
     *  this function activates the audio source, when the collider is exited
     */
    private void OnTriggerExit(Collider other)
    {
        triggerSound.Play();
    }
}
