using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

public class EnemyHit : MonoBehaviour
{
    public AudioRoundRobin deathAudioScript;
    [SerializeField]
    private GameObject explosion;

    private Animator a;
    private bool animatorExists = false;

    [SerializeField]
    private GameObject engineExhaustLeft;
    [SerializeField]
    private GameObject engineExhaustRight;

    void Start()
    {
        explosion = transform.GetChild(0).gameObject;

        animatorExists = explosion.TryGetComponent<Animator>(out a);

        if (animatorExists) a.enabled = false;

        explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void OnDestroy()
    {
        Debug.LogFormat("{0} Destroyed...", MethodBase.GetCurrentMethod());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Pewnie da się zrobić to lepiej, ale tak najszybciej uniemożliwiłem dwóm różnym przeciwnikom zestrzelenie się nawzajem.
        if (animatorExists && !collision.collider.CompareTag("Enemy Missile"))
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;

            DestroyEngineExhausts();

            a.enabled = true;
            GetComponent<SpriteRenderer>().enabled = false;
            a.Play("Base Layer.Explosion");
            explosion.transform.SetParent(transform.parent);

            deathAudioScript.RoundRobinPlay(0.45f);
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void DestroyEngineExhausts()
    {
        Destroy(engineExhaustLeft);
        Destroy(engineExhaustRight);
    }
}
