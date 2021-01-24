using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Linq;
using System;

public class EnemyWaveActivator : MonoBehaviour
{
    public GameObject enemy;
    
    [Range(0.5f, 5.0f)]
    public float delay;
    [Range(1,5)]
    public int enemiesToSpawn;

    private bool spawned = false;
    private GameManager gameManager;
    private int enemiesToSpawnSave;

    void Start()
    {
        gameManager = gameManager = this.gameObject.GetGameManager();
        enemiesToSpawnSave = enemiesToSpawn;
        GameManager.OnRestartSector += GameManager_OnRestartSector;
    }

    private void GameManager_OnRestartSector(object sender, EventArgs e)
    {
        enemiesToSpawn = enemiesToSpawnSave;
        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Wheel") && !spawned)
        {
            InstantiateEnemyWave();
            spawned = true;
        }
    }

    private void InstantiateEnemyWave()
    {
        //GameObject go = Instantiate(enemy) as GameObject;
        StartCoroutine(SpawnAnotherEnemy());
    }

    private IEnumerator SpawnAnotherEnemy()
    {
        while (enemiesToSpawn > 0)
        {
            yield return new WaitForSeconds(delay);
            GameObject go = Instantiate(enemy, gameManager.enemyContainer.transform) as GameObject;
            enemiesToSpawn--;
        }
    }
}
