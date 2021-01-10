using UnityEngine;
using System.Reflection;

public class GroundScroller : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private EdgeCollider2D ec;

    private float width;
    private bool colliderExists = false;

    [Range(0f, 5f)]
    public float scrollSpeed = 3.0f;

void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = new Vector2(-scrollSpeed, 0);

        width = GetWidth();
    }

    void Update()
    {
        Debug.LogFormat("{0} Ground x position is {1}", MethodBase.GetCurrentMethod(), transform.position.x);

        if (transform.position.x + width / 2.0f - 1.0f < -width)
        {
            Reposition();
            Debug.LogFormat("Reposition ground tile {0} now; width is {1}; x is {2}.", this.name, width, transform.position.x);
        }
        rb.velocity = new Vector2(-scrollSpeed, 0);
    }

    private void Reposition()
    {
        Vector2 newPosition = new Vector2(width * 2.0f, 0);
        transform.position = (Vector2) transform.position + newPosition;
    }

    public void ChangeScrollSpeed(float speed)
    {
        this.scrollSpeed = speed;
        rb.velocity = new Vector2(-scrollSpeed, 0);
    }

    private float GetWidth()
    {
        colliderExists = TryGetComponent<EdgeCollider2D>(out ec);

        if (colliderExists)
        {
            return ec.bounds.size.x;
        }

        return 0.0f;
    }
}
