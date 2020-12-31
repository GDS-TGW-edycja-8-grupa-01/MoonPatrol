using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float range;
    public float magnitude;
    public float speed;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 origin;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        origin = transform.position;

        return;
    }

    void Update()
    {
        Vector3 direction;

        if (transform.position.x >= origin.x + range)
        {
            direction = Vector3.left;
        }
        else
        {
            direction = Vector3.right;
        }

        transform.position += direction * Time.deltaTime * speed;
    }
}
