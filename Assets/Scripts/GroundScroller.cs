using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundScroller : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    
    private float width = 16.0f;

    [Range(1f, 5f)]
    public float speed = 3.0f;
    
void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = new Vector2(-speed, 0);
    }

    void Update()
    {
        if (transform.position.x < -width)
        {
            Reposition();
            Debug.LogFormat("Reposition ground tile {0} now; width is {1}.", this.name, width);
        }
    }

    private void Reposition()
    {
        Vector2 newPosition = new Vector2(width * 2.0f, 0);
        transform.position = (Vector2) transform.position + newPosition;
    }
}
