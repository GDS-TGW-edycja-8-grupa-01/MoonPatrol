using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class SectorMarker : MonoBehaviour
{
    private GameManager gameManager;
    private SpriteRenderer sr;

    private void Start()
    {
        gameManager = this.gameObject.GetGameManager();

        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "FrontWheel") return;

        int length = sr.sprite.name.Length;
        string sectorName = sr.sprite.name.Substring(length - 1, 1);

        gameManager.CompleteSector(sectorName);
    }
}
