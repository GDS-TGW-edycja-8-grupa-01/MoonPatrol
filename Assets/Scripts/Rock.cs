﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Rock : MonoBehaviour
{
    public AudioRoundRobin rockAudioScript;

    private Animator a;
    private bool animatorExists = false;
    private GameObject explosion;
    
    public static event EventHandler OnRockDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        explosion = transform.Find("Explosion").gameObject;

        animatorExists = explosion.TryGetComponent<Animator>(out a);

        if (animatorExists) a.enabled = false;

        explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string[] collidable = { "Player", "Wheel", "Player Missile" };

        if (!collidable.Contains(collision.gameObject.tag))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player Missile"))
        {
            GameObject jc = GameObject.Find("JumpCollider");

            jc.GetComponent<EdgeCollider2D>().enabled = false;


            Destroy(collision.gameObject);

            
            GetComponent<PolygonCollider2D>().enabled = false;
            OnRockDestroyed?.Invoke(this, EventArgs.Empty);
        }

        if (animatorExists)
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;

            a.enabled = true;
            a.Play("Base Layer.Explosion");

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;

            rockAudioScript.PlayFromArray(0, 0.5f, 0.3f);
            Destroy(this.gameObject, delay);
            //Destroy(collision.gameObject);
        }
    }
}

