using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMissile : MonoBehaviour
{
    
    [Range(0.1f, 10f)]
    public float gravity = 1f;
    [Range(0.01f, 5.0f)]
    public float accuracy = 0.5f;
    //publiczna zmienna która przechwytuje wektor kierunku ruchu spodka, który stworzył pocisk
    [HideInInspector]
    public Vector2 direction;
    private GameObject player;
    private GameObject explosion;
    private Rigidbody2D rb;
    private Animator a;
    private bool animatorExists = false;
    private bool exploded = false;
    private Vector2 movementVector;
    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private float timeOfFall;
    private float counter = 0;
    private TrailRenderer tr;
    

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

        //animacja?
        explosion = transform.GetChild(0).gameObject;
        animatorExists = explosion.TryGetComponent<Animator>(out a);
        if (animatorExists) a.enabled = false;
        //explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        rb.useFullKinematicContacts = true;
        startingPosition = transform.position;
        player = GameObject.Find("Player");
     
        
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == 0)
        {
            //Krzywa pocisku
            if (!player)
            {
                targetPosition = new Vector3(transform.position.x, 0.0f, 0.0f);
            }
            else
            {
                targetPosition = player.transform.position + new Vector3(Random.Range(-accuracy, accuracy), 0.0f, 0.0f);
            }
            timeOfFall = Mathf.Sqrt(Mathf.Abs(targetPosition.y - startingPosition.y) / gravity);
            float speed = Mathf.Abs(targetPosition.x - startingPosition.x) / timeOfFall;
            if (direction.x > 0)
            {
                if (targetPosition.x < startingPosition.x)
                {
                    movementVector.x = 0;
                }
                else
                {
                    movementVector.x = speed;
                }
            }
            else
            {
                if (targetPosition.x < startingPosition.x)
                {
                    movementVector.x = -speed;
                }
                else
                {
                    movementVector.x = 0;
                }
            }
            movementVector.y = 0.0f;

        }
        else
        {
            //movementVector = new Vector2(movementVector.x, movementVector.y);
            movementVector.y -= 2 * gravity * Time.deltaTime;
            rb.transform.Translate(movementVector * Time.deltaTime);
        }
         
        counter += Time.deltaTime;
    }

    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            Debug.Log("EnemyMissile collided with: " + other.tag);
            missileExplosion();
        }
    }

    void missileExplosion() 
    {
        if (animatorExists)
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;
            a.enabled = true;
            exploded = true;
            GetComponent<SpriteRenderer>().enabled = false;
            tr.emitting = false;
            a.Play("Base Layer.Explosion");
            //rb.velocity = Vector2.zero;
            
            transform.DetachChildren();
            Destroy(this.gameObject, delay);
            Destroy(explosion.gameObject, delay);
        }
       
    }
}
