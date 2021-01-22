using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveActivator : MonoBehaviour
{
    public GameObject enemy;
    
    [Range(0.5f, 5.0f)]
    public float delay;
    [Range(1,5)]
    public int enemiesToSpawn;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wheel")
        {
            InstantiateEnemyWave();
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
            GameObject go = Instantiate(enemy) as GameObject;
            enemiesToSpawn--;
        }
    }
}
