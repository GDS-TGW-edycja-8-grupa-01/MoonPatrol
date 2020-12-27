using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorMarker : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Sprite[] spriteSheet;

    private void Start()
    {
        transform.position = new Vector2(-9.00f, 0.00f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.position * 1.00f;

        sr = GetComponent<SpriteRenderer>();
        spriteSheet = Resources.LoadAll<Sprite>("keys");
    }

    void AssignLetter(char markerSign)
    {
        sr.sprite = spriteSheet[0];
    }

    private void OnBecameInvisible()
    {
        Destroy(this);
    }
}
