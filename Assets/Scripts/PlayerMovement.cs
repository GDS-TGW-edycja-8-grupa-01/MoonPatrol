﻿using UnityEngine;
using System.Reflection;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool spawned = false;
    private bool isJumping = false;
    private Vector2 movement;
    
    public GameObject spawn;
    
    [SerializeField]
    public LayerMask groundLayerMask;
    [Range(1, 5.0f)]
    public float accelerationRate = 1.0f;
    [Range(1, 5.0f)]
    public float decelerationRate = 1.0f;
    [Range(0, 1000.0f)]
    public float jumpForce = 20.0f;
    [Range(0, 10.0f)]
    public float forwardSpeed = 3.0f;
    [Range(0.1f, 1.0f)]
    public float groundCheckDistance = 0.0f;

    void Start()
    {
        transform.position = spawn.transform.position;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.right * accelerationRate;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.left * decelerationRate;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Jump")))
        {
            if (IsGrounded())
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity += Vector2.up * jumpForce;
            }
        }
    }

    private bool IsGrounded()
    {
        Vector3 start = sr.bounds.center;
        Vector3 direction = Vector3.down * groundCheckDistance;
        bool returnValue = false;

        RaycastHit2D hit = Physics2D.Raycast(start, Vector3.down, groundCheckDistance, groundLayerMask);

        returnValue = hit.collider != null;

        Color rayColor;
        if (returnValue)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(start, direction, rayColor);

        Debug.LogFormat("{0}: {1}", MethodBase.GetCurrentMethod(), hit.collider);

        return returnValue;
    }
}
