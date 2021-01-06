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

    [Range(0f, 5f)]
    public float scrollSpeed = 3.0f;
    
void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = new Vector2(-scrollSpeed, 0);
    }

    void Update()
    {
        if (transform.position.x < -width && 1 == 0)
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

    public void ChangeScrollSpeed(float speed)
    {
        this.scrollSpeed = speed;
        rb.velocity = new Vector2(-scrollSpeed, 0);
    }
}
