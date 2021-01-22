using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Ragdoll : MonoBehaviour    
{
    [Range(0.0f, 2.0f)]
    public float explosionDelay = 0.5f;

    private Rigidbody2D rb;
    private Rigidbody2D[] rbChildren;
    private bool exploded = false;
    private Vector3 bounds;
    // Start is called before the first frame update
    void Start()
    {
        bounds = MonoBehaviourExtensions.GetScreenBounds(this);
        Physics2D.IgnoreLayerCollision(10, 12);
        rb = gameObject.GetComponent<Rigidbody2D>();
        rbChildren = new Rigidbody2D[3];
        for(int i = 0; i < 3; i++)
        {
            rbChildren[i] = gameObject.transform.GetChild(i).GetComponent<Rigidbody2D>();
        }
        //udajemy, że jest to prędkość pojadu, bez tego pojazd zatrzymuje się na dziurze
        rb.velocity = Vector2.right * 20.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLIDED : " + collision.gameObject.tag);
        if ((collision.gameObject.CompareTag("GroundForRagdoll")) && (!exploded))
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);
        float forceFactor = (transform.position.x + bounds.x)/-bounds.x;
        //eksplozja pojazdu po kontakcie kadłuba z dziurą
        rb.AddForceAtPosition(Vector2.up * 20.0f * forceFactor, new Vector2(transform.position.x - 5.0f, transform.position.y), ForceMode2D.Impulse);
        for (int i = 0; i < 3; i++)
        {
            rbChildren[i].AddForce(new Vector2(i * 1.0f - 1.0f, 1.0f) * 8.0f, ForceMode2D.Impulse);
        }
        exploded = true;
        Debug.Log("EXPLODED !");
        StopCoroutine(Explode());
    }
}
