using UnityEngine;
using System.Reflection;

public class EnemyHit : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private Animator a;
    private bool animatorExists = false;

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

            a.enabled = true;
            GetComponent<SpriteRenderer>().enabled = false;
            a.Play("Base Layer.Explosion");

            Destroy(this.gameObject, delay);
            Destroy(collision.gameObject);
        }
    }
}
