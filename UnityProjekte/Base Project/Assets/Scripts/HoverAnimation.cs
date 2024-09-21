using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    public float amplitude = 2f;
    public float speed = 0.5f;

    private void Update()
    {
        Vector3 p = transform.position;
        p.y = amplitude * Mathf.Cos(Time.time * speed);
        transform.position = p;
    }
}
