using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLeft : MonoBehaviour
{
    //GameObject which shold be rotated (Drag&Drop Parameter)
    public GameObject toRotate;
    
    /*
     *Functions which rotates the specified Object while Collider is entered (left)
     */
    private void OnTriggerStay(Collider other)
    {
        toRotate.transform.Rotate(0.0f, 0.0f, -1.5f,Space.Self);
    }
}
