using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyHit : MonoBehaviour
{
    public AudioRoundRobin deathAudioScript;
    [SerializeField]
    private GameObject explosion;
    private GameObject groundExplosion;

    private Animator a;
    private Animator b;
    private bool animatorExists = false;
    private bool anotherAnimatorExists = false;
    [SerializeField]
    private GameObject engineExhaustLeft;
    [SerializeField]
    private GameObject engineExhaustRight;

    public static event EventHandler OnEnemyDestroyed;

    void Start()
    {
        explosion = transform.GetChild(0).gameObject;
        groundExplosion = transform.GetChild(1).gameObject;
        
        animatorExists = explosion.TryGetComponent<Animator>(out a);
        anotherAnimatorExists = groundExplosion.TryGetComponent<Animator>(out b);
        if (animatorExists && anotherAnimatorExists)
        {
            a.enabled = false;
            b.enabled = false;
        }

        explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void OnDestroy()
    {
        Debug.LogFormat("{0} Destroyed...", MethodBase.GetCurrentMethod());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        string[] deadlyThreats = { "Player Missile", "Player", "Wheel", "Ground" };
        //Pewnie da się zrobić to lepiej, ale tak najszybciej uniemożliwiłem dwóm różnym przeciwnikom zestrzelenie się nawzajem.
        if (animatorExists && deadlyThreats.Contains(collider.tag))
        {

            DestroyEngineExhausts();
            if (collider.CompareTag("Ground"))
            {
                b.enabled = true;
                float delay = b.GetCurrentAnimatorClipInfo(0).Length;
                b.Play("Base Layer.Explosion");
                groundExplosion.transform.SetParent(transform.parent);
                Destroy(groundExplosion, delay);
            }
            else
            {
                a.enabled = true;
                float delay = a.GetCurrentAnimatorClipInfo(0).Length;
                a.Play("Base Layer.Explosion");
                explosion.transform.SetParent(transform.parent);
                Destroy(explosion, delay);
            }

            GetComponent<SpriteRenderer>().enabled = false;
            
            

            deathAudioScript.RoundRobinPlay(0.45f);
            if (collider.CompareTag("Player Missile"))
            {
                Destroy(collider.gameObject);
                OnEnemyDestroyed?.Invoke(this, EventArgs.Empty);
            }
            Destroy(this.gameObject);
            
        }
    }

    private void DestroyEngineExhausts()
    {
        Destroy(engineExhaustLeft);
        Destroy(engineExhaustRight);
    }
}
