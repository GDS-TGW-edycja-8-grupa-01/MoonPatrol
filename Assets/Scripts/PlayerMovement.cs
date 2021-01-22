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
    [Range(3.0f, 10.0f)]
    public float scrollSpeed = 5.0f;

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
    private float[] backgroundScrollSpeed;

    void Start()
    {
        transform.position = spawn.transform.position;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        screenBounds = this.GetScreenBounds();
        width = GetComponent<SpriteRenderer>().bounds.size.x;

        Debug.LogFormat("{0} Screen bounds are: {1}", MethodBase.GetCurrentMethod(), screenBounds);
        backgroundScrollSpeed = new float[background.transform.childCount];
        
        for(int i = 0; i < background.transform.childCount; i++) 
        {
            backgroundScrollSpeed[i] = background.transform.GetChild(i).GetComponent<BackgroundScroller>().scrollSpeed;
        }
        return;
    }

    void Update()
    {
        //Sterowanie
        Debug.Log("FPS : " + (1 / Time.deltaTime).ToString());
        Debug.Log("LAST ACC : " + lastAcceleration.ToString());
        //Nie rozumiem jak to działa, ale faktycznie działa
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        
        //Input spięty w if else if ... else, tak by odwzorować poruszanie się w oryginale
        //Jednak nie możemy się poruszać lewo-prawo w trakcie skoku, ale jeżeli przed skokiem przyśpieszaliśmy/zwalnialiśmy, to będzie to kontynuowane w locie
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
            
            lastAcceleration = accelerationRate;
            
        }
        else if ((Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0) && IsGrounded())
        {

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.left * decelerationRate;
            
            lastAcceleration = -decelerationRate;
            
        }
        else
        {
            //if ((Input.GetKeyUp(KeyCode.A) || (Input.GetKeyUp(KeyCode.D))))
            if(IsGrounded())
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                
                //Gdy nie ma w danej klatce inputu od gracza 
                //i gdy pojazd znajduje się na prawo od spawna - po mału zwalnia, gdy na lewo - przyśpiesza. Ściąga nas do punktu nominalnego na ekranie.
                
                if (transform.position.x - spawn.transform.position.x > 0)
                {
                    rb.velocity += Vector2.left * inertia / 10.0f;
                    lastAcceleration = -inertia / 10.0f;
                //Gdy zwalniamy samoistnie, musi podążać za nami prędkość świata
                    
                }
                //To bardzo ciekawe, ale okazuje się, że porównywanie floatów to problem, nawet jeżeli mamy do tego dedykowane funkcje w Unity xD
                else if(Mathf.Approximately(Mathf.Round(transform.position.x * 100), Mathf.Round(spawn.transform.position.x * 100)))
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    lastAcceleration = 0;
                    
                }
                else
                {
                    rb.velocity += Vector2.right * inertia / 10.0f;
                    lastAcceleration = inertia / 10.0f;
                //Gdy przyśpieszamy samoistnie, musi podążać za nami prędkość świata
                    
                }
            }
        }
        //Przez cały czas zbieramy informację o przyśpieszeniu, tylko po to by móc skorzystać z niego w tej funkcji, która podtrzymuje przyśpieszenie podczas lotu po skoku
        if (!IsGrounded())
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity += Vector2.right * lastAcceleration;   
        }

        if (ShouldChangeBackroundScrollSpeed())
        {
            ChangeBackgroudScrollSpeed(background);
            ChangeRollingScrollSpeed(ground);
            ChangeRollingScrollSpeed(obstacles);
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
    private void ChangeBackgroudScrollSpeed(GameObject go)
    {
        for (int i = 1; i < go.transform.childCount; i++)
        {
            float newSpeed = backgroundScrollSpeed[i] + (transform.position.x - spawn.transform.position.x) / (-spawn.transform.position.x - width / 2.0f) * groundScrollAccelerationRate;

            go.transform.GetChild(i).GetComponent<BackgroundScroller>().scrollSpeed = newSpeed;
        }

    }

    private void ChangeRollingScrollSpeed(GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            float newSpeed = scrollSpeed + ((transform.position.x - spawn.transform.position.x) / (-spawn.transform.position.x - width / 2.0f)) * groundScrollAccelerationRate;

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
