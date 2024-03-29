﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    [Range(0.0f, 3.0f)]
    public float offset = 0.0f;

    private Vector3 bounds;
    private float width;

    private void Awake()
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

        position.x = Mathf.Clamp(position.x, -bounds.x + width / 2.0f + offset, 0.0f - width / 2.0f - offset);

        transform.position = position;
    }

    public Vector2 GetBounds()
    {
        return new Vector2(-bounds.x + width / 2.0f, 0.0f - width / 2.0f);
    }
}
