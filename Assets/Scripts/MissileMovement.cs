using UnityEngine;
using System.Reflection;

public class MissileMovement : MonoBehaviour
{
    [SerializeField]
    public FiringDirection firingDirection;
    private GameObject explosion;

    [Range(1, 100f)]
    public float speed = 1f;

    private Rigidbody2D rb;
    private Animator a;
    private bool animatorExists = false;

    [Range(1f, 10f)]
    public float range = 6f;
    private float originX;
    //[Range(-1.0f, 1.0f)]
    //public float animationScaleFactor = 1.0f;
    private AudioRoundRobin audioScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        explosion = transform.GetChild(0).gameObject;

        animatorExists = explosion.TryGetComponent<Animator>(out a);

        if (animatorExists) a.enabled = false;

        originX = transform.position.x;

        explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        audioScript = GetComponent<AudioRoundRobin>();
    }

    void Update()
    {
        Vector2 salvo = firingDirection == FiringDirection.Horizontal ? Vector2.right : Vector2.up;

        rb.velocity = salvo * speed;

        if (firingDirection == FiringDirection.Horizontal && transform.position.x >= originX + range)
        {
            explode();
        }
    }

    private void explode()
    {
        if (animatorExists)
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;
            explosion.transform.SetParent(transform.parent);
            a.enabled = true;
            GetComponent<SpriteRenderer>().enabled = false;
            a.Play("Base Layer.Explosion");
            rb.velocity = Vector2.zero;
            audioScript.RoundRobinPlay(0.25f);
            Destroy(this.gameObject);
            Destroy(explosion, delay);
            animatorExists = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Rock"))
        {
            explode();
        }
    }


    public enum FiringDirection
    {
        Horizontal,
        Vertical
    }

    private void OnDestroy()
    {
        Debug.LogFormat("{0} Destroyed...", MethodBase.GetCurrentMethod());
        
    }
}
