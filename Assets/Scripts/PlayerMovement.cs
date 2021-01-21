using UnityEngine;
using System.Reflection;
using ExtensionMethods;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 movement;

    public GameObject spawn;

    //[SerializeField]
    public LayerMask groundLayerMask;
    [Range(1.0f, 5.0f)]
    public float accelerationRate = 1.0f;
    [Range(1.0f, 5.0f)]
    public float decelerationRate = 1.0f;
    [Range(0.0f, 1000.0f)]
    public float jumpForce = 20.0f;
    [Range(0.0f, 10.0f)]
    public float forwardSpeed = 3.0f;
    [Range(0.1f, 1.0f)]
    public float groundCheckDistance = 0.0f;
    [Range(1.0f, 10.0f)]
    public float groundScrollAccelerationRate;
    [Range(0.0f, 10.0f)]
    public float inertia = 0;

    //[SerializeField]
    public GameObject background;
    //[SerializeField]
    public GameObject ground;
    //[SerializeField]
    public GameObject obstacles;

    private Vector3 screenBounds;
    private float width;
    private float lastAcceleration = 0.0f;
    private Vector2 velocity = Vector2.zero;
    void Start()
    {
        transform.position = spawn.transform.position;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        screenBounds = this.GetScreenBounds();
        width = GetComponent<SpriteRenderer>().bounds.size.x;

        Debug.LogFormat("{0} Screen bounds are: {1}", MethodBase.GetCurrentMethod(), screenBounds);

        return;
    }

    void Update()
    {
        
        Debug.Log("LAST ACC : " + lastAcceleration.ToString());
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        Debug.Log("FPS : " + (1 / Time.deltaTime).ToString());
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Jump")) && IsGrounded())
        {
            if (true)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity += Vector2.up * jumpForce;
            }
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0) && IsGrounded())
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.right * accelerationRate;

            if (ShouldChangeBackroundScrollSpeed())
            {
                ChangeBackgroudScrollSpeed(background, groundScrollAccelerationRate);
                ChangeRollingScrollSpeed(ground, groundScrollAccelerationRate);
                ChangeRollingScrollSpeed(obstacles, groundScrollAccelerationRate);
            }
            lastAcceleration = accelerationRate;
            
        }
        else if ((Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0) && IsGrounded())
        {

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.left * decelerationRate;

            if (ShouldChangeBackroundScrollSpeed())
            {
                ChangeBackgroudScrollSpeed(background, -groundScrollAccelerationRate);
                ChangeRollingScrollSpeed(ground, -groundScrollAccelerationRate);
                ChangeRollingScrollSpeed(obstacles, -groundScrollAccelerationRate);
            }
            lastAcceleration = -decelerationRate;
            
        }
        else
        {
            //if ((Input.GetKeyUp(KeyCode.A) || (Input.GetKeyUp(KeyCode.D))))
            if(IsGrounded())
            {
                lastAcceleration = 0;
                //rb.velocity = Vector2.zero;
            }
        }

        if (!IsGrounded())
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.right * lastAcceleration;
            if (ShouldChangeBackroundScrollSpeed() && (lastAcceleration != 0))
            {
                float factor = Mathf.Abs(lastAcceleration) / lastAcceleration;
                ChangeBackgroudScrollSpeed(background, groundScrollAccelerationRate * factor);
                ChangeRollingScrollSpeed(ground, groundScrollAccelerationRate * factor);
                ChangeRollingScrollSpeed(obstacles, groundScrollAccelerationRate * factor);
            }
            
        }

    }

    private void ChangeVelocity()
    {
        if (inertia != 0)
        {
            float oldY = velocity.y;
            rb.velocity += velocity * Time.deltaTime / inertia;
            rb.velocity.Set(rb.velocity.x, oldY);
            //rb.velocity = new Vector2(Mathf.Min(rb.velocity.x, velocity.x), Mathf.Min(rb.velocity.y, velocity.y));
        }
    }

    private bool IsGrounded()
    {
        Vector3 start = sr.bounds.center;
        Vector3 direction = Vector3.down * groundCheckDistance;
        bool returnValue = false;

        RaycastHit2D hit = Physics2D.Raycast(start, Vector3.down, groundCheckDistance, groundLayerMask);

        returnValue = hit.collider != null;
        /*
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

        Debug.LogFormat("{0}: {1}", MethodBase.GetCurrentMethod(), hit.collider); */

        return returnValue;
    }
    //GetComponent i GetChild w pętli Update - fpsy spadają
    private void ChangeBackgroudScrollSpeed(GameObject go, float rate)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            float scrollSpeed = go.transform.GetChild(i).GetComponent<BackgroundScroller>().scrollSpeed;
            float newSpeed = scrollSpeed * (1 + rate / 10 * Time.deltaTime);

            go.transform.GetChild(i).GetComponent<BackgroundScroller>().scrollSpeed = newSpeed;
        }
    }

    private void ChangeRollingScrollSpeed(GameObject go, float rate)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            float scrollSpeed = go.transform.GetChild(i).GetComponent<GroundScroller>().scrollSpeed;
            float newSpeed = scrollSpeed * (1 + rate / 10 * Time.deltaTime);

            go.transform.GetChild(i).GetComponent<GroundScroller>().scrollSpeed = newSpeed;
        }
    }

    private bool ShouldChangeBackroundScrollSpeed()
    {
        float x = transform.position.x;
        
        Debug.LogFormat("{0}; postion.x is {1}; player width is {2}; screen width is {3}.", MethodBase.GetCurrentMethod(), x, width, screenBounds.x);

        if ((x - width / 2.0f <= -screenBounds.x) || (x + width / 2.0f >= 0.0f))
        {
            return false;
        }

        return true;
    }
}
