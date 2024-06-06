using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMeCollisionWorldCoordinates : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        
        //Only WORKS FOR WORLD (Simpleworld2)
        ContactPoint cord = collision.contacts[0];
        Debug.Log("Contactpoint is " + cord);
        Destroy(GameObject.FindWithTag("World")); //We can change the name of SimpleWorld2 without needing to change the code
    }
}