using System.Collections;
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
    private float delay;

    public static event EventHandler OnRockDestroyed;

    void Start()
    {
        explosion = transform.Find("Explosion").gameObject;

        animatorExists = explosion.TryGetComponent<Animator>(out a);

        if (animatorExists) a.enabled = false;

        explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameManager.OnRestartSector += GameManager_OnRestartSector;
    }

    private void GameManager_OnRestartSector(object sender, EventArgs e)
    {
        SetActive(true);
    }

    private void Hide()
    {
        if (animatorExists)
        {
            delay = a.GetCurrentAnimatorClipInfo(0).Length;

            a.enabled = true;
            a.Play("Base Layer.Explosion");

            rockAudioScript.PlayFromArray(0, 0.5f, 0.3f);

            SetActive(false);
        }
    }

    private void SetActive(bool isActive)
    {
        GetComponent<SpriteRenderer>().enabled = isActive;
        GetComponent<PolygonCollider2D>().enabled = isActive;

        GameObject jc = GameObject.Find("JumpCollider");
        jc.GetComponent<EdgeCollider2D>().enabled = isActive;
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
            Destroy(collision.gameObject);

            Hide();

            OnRockDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}

