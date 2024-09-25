using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRight : MonoBehaviour
{
    public GameObject toRotate;

    /*
     *Functions which rotates the specified Object while Collider is entered (right)
     */
    private void OnTriggerStay(Collider other)
    {
        toRotate.transform.Rotate(0.0f, 0.0f, 1.5f,Space.Self);
    }
}
