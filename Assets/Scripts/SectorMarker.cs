using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorMarker : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 bounds;

    private void Start()
    {
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        transform.position = new Vector2(bounds.x, -4.55f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * 4.00f;

        sr = GetComponent<SpriteRenderer>();
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
