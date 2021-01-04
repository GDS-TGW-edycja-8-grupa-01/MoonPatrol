using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [Range(1, 100f)]
    public float speed = 1f;
    //publiczna zmienna która przechwytuje wektor kierunku ruchu spodka, który stworzył pocisk
    [HideInInspector]
    public Vector2 direction = Vector2.down;
    
    private GameObject explosion;
    private Rigidbody2D rb;
    private Animator a;
    private bool animatorExists = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //animacja?
        explosion = transform.GetChild(0).gameObject;
        animatorExists = explosion.TryGetComponent<Animator>(out a);
        if (animatorExists) a.enabled = false;
        //explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        rb.useFullKinematicContacts = true;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Vector2.left, ForceMode2D.Force);
        //Vector2 direction = Vector2.down;
        //rb. = direction * speed;

        rb.transform.Translate((new Vector2(direction.x, 0f) + Vector2.down) * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("EnemyMissile collided with: " + other.collider.tag);

        missileExplosion();
    }

    void missileExplosion() 
    {
        if (animatorExists)
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;

            a.enabled = true;
            GetComponent<SpriteRenderer>().enabled = false;
            a.Play("Base Layer.Explosion");
            rb.velocity = Vector2.zero;

            Destroy(this.gameObject, delay);   
        }
       
    }
}
