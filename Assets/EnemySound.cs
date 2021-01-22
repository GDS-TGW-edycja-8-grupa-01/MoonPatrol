using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioRoundRobin enemySoundScript;
    private void Awake()
    {
        enemySoundScript.RoundRobinPlay(0.3f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
