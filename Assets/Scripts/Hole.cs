using UnityEngine;
using ExtensionMethods;
using System.Linq;
using System;

public class Hole : MonoBehaviour
{
    [HideInInspector]
    public bool dontSubscribe = false;
    private bool playerKilled = false;
    private void Start()
    {
        if (!dontSubscribe) GameManager.OnRestartSector += GameManager_OnRestartSector;
    }

    private void GameManager_OnRestartSector(object sender, EventArgs e)
    {
        GameObject jc = transform.parent.transform.Find("JumpCollider").gameObject;
        jc.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        playerKilled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerKilled)
        {
            bool collidedWithPlayer;
            GameObject player;
            PlayerHit playerHit;
            bool playerHitExists;
            collidedWithPlayer = collision.gameObject.CompareTag("Wheel");
            if (collidedWithPlayer)
            {
                player = collision.gameObject.transform.parent.gameObject;



                playerHitExists = player.TryGetComponent<PlayerHit>(out playerHit);

                if (playerHitExists && collidedWithPlayer)
                {
                    playerHit.DieHole(this.gameObject);
                    playerKilled = true;
                }
            }
        }
    }
}
