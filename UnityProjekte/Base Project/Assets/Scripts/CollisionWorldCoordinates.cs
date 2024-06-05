using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWorldCoordinates : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(CollisionWorldCoordinates collision) {

        ContactPoint 3DCoordinate = collision.contacts[0];
        Debug.Log("Contactpoint is " + 3DCoordinate);
    }
}
