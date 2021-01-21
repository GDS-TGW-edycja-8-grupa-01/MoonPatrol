using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject background;
    private GameObject backgroundScroller;

    public GameObject ground;
    public GameObject obstacles;
    private GameObject groundScroller;

    public GameObject player;

    public string currentSector = "a";
    public int startedSectorsCount = 0;
    public string[] startedSectors;

    [Range(0.0f, 10.0f)]
    public float restartLevelDelay = 2.0f;

    [Range(1, 999)]
    public int remaingingLivesCount = 1;

    private Vector3 placeOfDeath;

    private void Start()
    {
        
    }

    public void Die(Vector2 placeOfDeath)
    {
        ChangeBackgroundScrollSpeed(0.0f);
        ChangeRollingScrollSpeed(ground, 0.0f);
        ChangeRollingScrollSpeed(obstacles, 0.0f);

        this.placeOfDeath = placeOfDeath;

        StartCoroutine(RestartLastSector());
    }
    //work in progress
    /*private IEnumerator StopScrollSpeeds(float time)
    {
        for(float i = time; i > 0; i - Time.deltaTime)
        {

        }
    }*/

    public void Respawn()
    {
        if (remaingingLivesCount == 0)
        {
            GameOver();
        }

        Vector3 respawn = GetRespawn();

        Instantiate(player, respawn, Quaternion.identity);


    }

    private Vector3 GetRespawn()
    {
        GameObject respawn;

        respawn = GameObject.Find("Spawn");

        return respawn.transform.position;
    }

    private void GameOver()
    {

    }

    private void ChangeBackgroundScrollSpeed(float speed)
    {
        foreach (GameObject go in background.transform.GetAllChildren())
        {
            BackgroundScroller bs = go.GetComponent<BackgroundScroller>();

            bs.scrollSpeed = speed;
        }

    }

    private void ChangeRollingScrollSpeed(GameObject rolling, float speed)
    {
        foreach (GameObject go in rolling.transform.GetAllChildren())
        {
            GroundScroller gs = go.GetComponent<GroundScroller>();

            //gs.ChangeScrollSpeed(0.0f);
            gs.scrollSpeed = speed;
        }

    }

    public void BeginSector(string sectorName)
    {
        if (!startedSectors.Contains(sectorName))
        {
            startedSectorsCount += 1;
            currentSector = sectorName;
            Array.Resize(ref startedSectors, startedSectorsCount);
            startedSectors[startedSectorsCount - 1] = sectorName;

            return;
        }

        return;


    }

    IEnumerator RestartLastSector()
    {
        yield return new WaitForSeconds(restartLevelDelay);

        ObstaclesRoller or = obstacles.GetComponent<ObstaclesRoller>();
        float y = -4.18f;
        float z = -2.0f;
        float restartXPosition = 8.0f;

        List<GameObject> levelsToComplete = GetLevelsToRollback(or);
        float previousLevelLength = restartXPosition;
        
        foreach (GameObject level in levelsToComplete)
        {
            level.transform.position = new Vector3(previousLevelLength, y, z);

            previousLevelLength += or.levelLength;
        }

        Respawn();
    }

    private List<GameObject> GetLevelsToRollback(ObstaclesRoller or)
    {
        List<GameObject> lastFew;
        int allSectorsCount = or.levels.Length;
        int sectorsToRollback = 0;

        List<GameObject> levels = or.transform.GetAllChildren();

        if (startedSectorsCount == 1)
        {
            return levels;
        }

        if (startedSectorsCount == levels.Count)
        {
            lastFew = new List<GameObject>();

            lastFew.Add(levels.Last());

            return lastFew;
        }

        levels.Reverse();

        sectorsToRollback = allSectorsCount - startedSectorsCount + 1;
        lastFew = levels.Take(sectorsToRollback).ToList();
        lastFew.Reverse();

        return lastFew;
    }

}
