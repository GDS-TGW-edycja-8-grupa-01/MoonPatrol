using UnityEngine;
using System.Reflection;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

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
    [Range(1, 10f)]
    public int groundScrollAccelerationRate;

    [SerializeField]
    public GameObject background;
    [SerializeField]
    public GameObject ground;
    [SerializeField]
    public GameObject obstacles;

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

            ChangeBackgroudScrollSpeed(background, groundScrollAccelerationRate);
            ChangeRollingScrollSpeed(ground, groundScrollAccelerationRate);
            ChangeRollingScrollSpeed(obstacles, groundScrollAccelerationRate);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.left * decelerationRate;

            ChangeBackgroudScrollSpeed(background, -groundScrollAccelerationRate);
            ChangeRollingScrollSpeed(ground, -groundScrollAccelerationRate);
            ChangeRollingScrollSpeed(obstacles, -groundScrollAccelerationRate);
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

    private void ChangeBackgroudScrollSpeed(GameObject go, float rate)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            float scrollSpeed = go.transform.GetChild(i).GetComponent<BackgroundScroller>().scrollSpeed;
            float newSpeed = scrollSpeed * (1 + rate / 1000);

            go.transform.GetChild(i).GetComponent<BackgroundScroller>().scrollSpeed = newSpeed;
        }
    }

    private void ChangeRollingScrollSpeed(GameObject go, float rate)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            float scrollSpeed = go.transform.GetChild(i).GetComponent<GroundScroller>().scrollSpeed;
            float newSpeed = scrollSpeed * (1 + rate / 1000);

            go.transform.GetChild(i).GetComponent<GroundScroller>().scrollSpeed = newSpeed;
        }
    }
}
