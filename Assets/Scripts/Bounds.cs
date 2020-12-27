using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    private Vector3 bounds;

    private void Start()
    {
        Debug.LogFormat("Screen dimensions are ({0}, {1})", Screen.width, Screen.height);

        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        Debug.LogFormat("Bounds are {0}", bounds);

    }
    private void LateUpdate()
    {
        Debug.LogFormat("My x position is {0}", transform.position.x);

        if (transform.position.x < bounds.x)
        {
            Debug.Log("Destroyed...");
            //Destroy(this);
        }
    }
}
