using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 bounds;
    [Range(-1.0f, 3.0f)]
    public float y;
    [Range(0.0f, 10.0f)]
    public float xOffset;

    private void Start()
    {
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        sr = GetComponent<SpriteRenderer>();

        transform.position = new Vector2(bounds.x + sr.size.x + xOffset, y);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * 4.00f;

        return;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            return;
        }
    }
}
