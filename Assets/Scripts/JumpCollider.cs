using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JumpCollider : MonoBehaviour
{
    public static event EventHandler OnJumpedOverObstacle;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "RearWheel")
        {
            OnJumpedOverObstacle?.Invoke(this, EventArgs.Empty);
        }
    }
}
