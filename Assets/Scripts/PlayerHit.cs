using UnityEngine;
using System.Linq;
using ExtensionMethods;

public class PlayerHit : MonoBehaviour
{
    public GameObject playerRagdoll;
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

        if ((!deadlyThreat.Contains(collision.tag)) || godMode)
        {
            return;
        }
        else
        {
            Die();
            Debug.Log("PLAYER COLLIDED WITH : " + collision.tag);
        }
    }

    public void Die()
    {
        if (animatorExists && (!godMode))
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;
            playerAudioScript.EngineSoundStop();

            a.enabled = true;
            explosion.transform.SetParent(transform.parent);

            GetComponent<SpriteRenderer>().enabled = false;
            a.Play("Base Layer.Explosion");

            gameManager.Die(transform.position);

            GameObject ragdoll = Instantiate(playerRagdoll, transform.position, transform.rotation);
            Destroy(ragdoll, gameManager.restartLevelDelay);
            Destroy(this.gameObject);
            Destroy(explosion, delay);

            transform.Find("FrontWheel").gameObject.SetActive(false);
            transform.Find("MiddleWheel").gameObject.SetActive(false);
            transform.Find("RearWheel").gameObject.SetActive(false);
            
        }
    }

    public void DieHole(GameObject hole)
    {
        if (animatorExists && (!godMode))
        {
            float delay = a.GetCurrentAnimatorClipInfo(0).Length;
            a.enabled = true;
            explosion.transform.SetParent(transform.parent);
            GameObject nextHole = new GameObject();
            if (hole.transform.GetSiblingIndex() <= hole.transform.childCount - 1)
            {
                nextHole = hole.transform.parent.GetChild(hole.transform.GetSiblingIndex() + 1).gameObject;
                nextHole.transform.GetChild(0).gameObject.SetActive(true);
            }
            playerAudioScript.EngineSoundStop();

            GetComponent<SpriteRenderer>().enabled = false;
            gameManager.Die(transform.position);
            hole.transform.GetChild(0).gameObject.SetActive(true);
            Physics2D.IgnoreLayerCollision(10, 12);
            GameObject ragdoll = Instantiate(playerRagdoll, transform.position, transform.rotation);
            Destroy(ragdoll, gameManager.restartLevelDelay);
            Destroy(this.gameObject);
            Destroy(explosion, delay);
            transform.Find("FrontWheel").gameObject.SetActive(false);
            transform.Find("MiddleWheel").gameObject.SetActive(false);
            transform.Find("RearWheel").gameObject.SetActive(false);

        }
    }
}
