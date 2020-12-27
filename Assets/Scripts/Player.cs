using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool spawned = false;
    private bool isJumping = false;
    private Vector2 movement;

    public GameObject spawn;

    [SerializeField]
    [Range(1, 5.0f)]
    public float accelerationRate = 1.0f;
    [Range(1, 5.0f)]
    public float decelerationRate = 1.0f;
    [Range(0, 1000.0f)]
    public float jumpForce = 20.0f;
    [Range(0, 10.0f)]
    public float forwardSpeed = 3.0f;

    void Start()
    {
        transform.position = spawn.transform.position;

        rb = GetComponent<Rigidbody2D>();

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

        if (Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Jump"))
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity += Vector2.up * jumpForce;
        }     
    }
}
