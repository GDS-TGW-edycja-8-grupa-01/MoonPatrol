using UnityEngine;
using System.Reflection;

public class GroundScroller : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    
    private float width = 23.0f;

    [Range(0f, 5f)]
    public float scrollSpeed = 3.0f;
    
void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = new Vector2(-scrollSpeed, 0);
    }

    void Update()
    {
        Debug.LogFormat("{0} Ground x position is {1}", MethodBase.GetCurrentMethod(), transform.position.x);

        if (transform.position.x < -width)
        {
            Reposition();
            Debug.LogFormat("Reposition ground tile {0} now; width is {1}.", this.name, width);
        }
    }

    private void Reposition()
    {
        Vector2 newPosition = new Vector2(16.0f * 2.0f, 0);
        transform.position = (Vector2) transform.position + newPosition;
    }

    public void ChangeScrollSpeed(float speed)
    {
        this.scrollSpeed = speed;
        rb.velocity = new Vector2(-scrollSpeed, 0);
    }
}
