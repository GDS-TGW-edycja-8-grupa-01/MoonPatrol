using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    private Vector3 bounds;
    private float width;

    private void Start()
    {
        Debug.LogFormat("Screen dimensions are ({0}, {1})", Screen.width, Screen.height);

        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        Debug.LogFormat("Bounds are {0}", bounds);

        width = GetComponent<SpriteRenderer>().bounds.size.x;
        
        Debug.LogFormat("Object width is {0}...", width);

    }
    private void LateUpdate()
    {
        Debug.LogFormat("My x position is {0}", transform.position.x);

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, -bounds.x + width / 2.0f, 0.0f - width / 2.0f);

        transform.position = position;
    }
}
