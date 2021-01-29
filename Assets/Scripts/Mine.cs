using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Mine : MonoBehaviour
{
    //public AudioRoundRobin mineAudioScript;

    private Animator a;
    private bool animatorExists = false;
    private GameObject explosion;
    private float delay;

    //public static event EventHandler OnMineDestroyed;

    void Start()
    {
        explosion = transform.Find("Explosion").gameObject;

        animatorExists = explosion.TryGetComponent<Animator>(out a);

        if (animatorExists) a.enabled = false;

        //explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        GameManager.OnRestartSector += GameManager_OnRestartSector;
    }

    // Update is called once per frame
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
            a.Play("Base Layer.GroundExplosion");

            //mineAudioScript.PlayFromArray(0, 0.5f, 0.3f);

            SetActive(false);
        }
    }

    private void SetActive(bool isActive)
    {
        GetComponent<SpriteRenderer>().enabled = isActive;
        GetComponent<PolygonCollider2D>().enabled = isActive;

        GameObject jc = transform.parent.transform.Find("JumpCollider").gameObject;
        jc.SetActive(isActive);

        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string[] collidable = { "Player", "Wheel"};

        if (!collidable.Contains(collision.gameObject.tag))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Wheel"))
        {
            Hide();
            //OnMineDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
