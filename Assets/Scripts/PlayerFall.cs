using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    [Range(1, 10.0f)]
    public float fallMultiplier = 3.5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.velocity.y < -0.0f) {
            Debug.LogFormat("fall {0}", rb.velocity);
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);
        }
    }
}
