using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    [SerializeField]
    public FiringDirection firingDirection;

    [Range(1, 100f)]
    public float speed = 1f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        Vector2 salvo = firingDirection == FiringDirection.Horizontal ? Vector2.right : Vector2.up;

        rb.velocity = salvo * speed;
    }

    public enum FiringDirection
    {
        Horizontal,
        Vertical
    }
}
