using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.gameObject;

        Debug.LogFormat("Ground collided with {0}.", collider.name);
    }

    
}
