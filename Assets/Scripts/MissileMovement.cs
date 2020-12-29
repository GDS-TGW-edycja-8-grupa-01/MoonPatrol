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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        explosion = transform.GetChild(0).gameObject;

        animatorExists = explosion.TryGetComponent<Animator>(out a);

        if (animatorExists) a.enabled = false;

        originX = transform.position.x;
    }
    
    void Update()
    {
        Vector2 salvo = firingDirection == FiringDirection.Horizontal ? Vector2.right : Vector2.up;

        rb.velocity = salvo * speed;

        if (firingDirection == FiringDirection.Horizontal && transform.position.x >= originX + range)
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
