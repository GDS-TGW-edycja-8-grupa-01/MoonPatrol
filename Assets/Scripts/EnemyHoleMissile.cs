using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class EnemyHoleMissile : MonoBehaviour
{
    
    [Range(0.1f, 10f)]
    public float gravity = 1f;
    [Range(0.01f, 5.0f)]
    public float accuracy = 0.5f;
    //publiczna zmienna która przechwytuje wektor kierunku ruchu spodka, który stworzył pocisk
    [HideInInspector]
    public Vector2 direction;
    //private GameObject player;
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
    private Vector3 bounds;
    private GameObject obstaclesRoller;
    private float objHeightHalved;

    public GameObject objToSpwn;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        //animacja
        explosion = transform.GetChild(0).gameObject;
        animatorExists = explosion.TryGetComponent<Animator>(out a);
        if (animatorExists) a.enabled = false;
        //explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        rb.useFullKinematicContacts = true;
        startingPosition = transform.position;
        //player = GameObject.Find("Player");
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        obstaclesRoller = GameObject.Find("ObstaclesRoller");
        objHeightHalved = objToSpwn.GetComponent<SpriteRenderer>().bounds.extents.y;

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
            
            targetPosition = new Vector3(bounds.x + Random.Range(-accuracy * 2, 0), -bounds.y, 0.0f);
            timeOfFall = Mathf.Sqrt(Mathf.Abs(targetPosition.y - startingPosition.y) / gravity);
            float speed = Mathf.Abs(targetPosition.x - startingPosition.x) / timeOfFall;
            movementVector.x = speed;
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
        Debug.Log("EnemyHoleMissile collided with: " + other.name);
        string[] collidables = { "Rock", "Ground" };
        if (collidables.Contains(other.tag))
        {
            
            missileExplosion();
            if (other.CompareTag("Ground"))
            {
                GameObject spawnedObject = Instantiate(objToSpwn, obstaclesRoller.transform.GetChild(0).transform, false);
                //-3.74f to magiczna liczba, którą wyliczyłem z sumarycznej pozycji y obiektów Hole w przykładowych prefabowych poziomach, jeżeli to się zmieni - trzeba będzie ją zmienić i tutaj
                spawnedObject.transform.position = new Vector3(transform.position.x, -3.74f, 0.0f);
            }
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
