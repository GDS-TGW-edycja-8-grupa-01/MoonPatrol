using UnityEngine;
using System.Linq;
using ExtensionMethods;

public class PlayerHit : MonoBehaviour
{
    public PlayerSound playerAudioScript;
    public bool godMode = false;
    private GameObject explosion;
    private GameManager gameManager;
    private Animator a;
    private bool animatorExists = false;
    

    void Start()
    {
        explosion = transform.Find("Explosion").gameObject;

        animatorExists = explosion.TryGetComponent<Animator>(out a);

        if (animatorExists) a.enabled = false;

        explosion.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        gameManager = this.gameObject.GetGameManager();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string[] deadlyThreat = { "Rock", "Enemy Missile" };

        if ((!deadlyThreat.Contains(collision.gameObject.tag)) || godMode)
        {
            return;
        }

        Die();
    }

    public void Die()
    {
        if (animatorExists && (!godMode))
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;
            playerAudioScript.EngineSoundStop();
            a.enabled = true;
            GetComponent<SpriteRenderer>().enabled = false;
            a.Play("Base Layer.Explosion");

            gameManager.Die(transform.position);

            transform.Find("FrontWheel").gameObject.SetActive(false);
            transform.Find("MiddleWheel").gameObject.SetActive(false);
            transform.Find("RearWheel").gameObject.SetActive(false);
            Destroy(this.gameObject, delay);

        }
    }
}
