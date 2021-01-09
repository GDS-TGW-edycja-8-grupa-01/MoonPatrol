using UnityEngine;
using ExtensionMethods;

public class Hole : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = this.gameObject.GetGameManager();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool collidedWithPlayer;
        GameObject player;
        PlayerHit playerHit;
        bool playerHitExists;

        player = collision.gameObject.transform.parent.gameObject;

        collidedWithPlayer = collision.gameObject.CompareTag("Wheel");

        playerHitExists = player.TryGetComponent<PlayerHit>(out playerHit);

        if (playerHitExists)
        {
            playerHit.Die();
        }
    }
}
