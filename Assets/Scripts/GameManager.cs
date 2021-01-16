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

    public string currentSector = "a";
    public int completedSectorsCount = 0;
    public string[] completedSectors;

    [Range(0.0f, 10.0f)]
    public float restartLevelDelay = 2.0f;

    private void Start()
    {
        return;
    }

    public void Die()
    {
        ChangeBackgroundScrollSpeed(0.0f);
        ChangeRollingScrollSpeed(ground, 0.0f);
        ChangeRollingScrollSpeed(obstacles, 0.0f);

        StartCoroutine(RestartLastSector());
    }

    private void ChangeBackgroundScrollSpeed(float speed)
    {
        foreach (GameObject go in background.transform.GetAllChildren())
        {
            BackgroundScroller bs = go.GetComponent<BackgroundScroller>();

            bs.scrollSpeed = speed;
        }

    }

    private void ChangeRollingScrollSpeed(GameObject rolling,  float speed)
    {
        foreach (GameObject go in rolling.transform.GetAllChildren())
        {
            GroundScroller gs = go.GetComponent<GroundScroller>();

            gs.ChangeScrollSpeed(0.0f);
        }

    }

    public void CompleteSector(string sectorName)
    {
        if (!completedSectors.Contains(sectorName))
        {
            completedSectorsCount += 1;
            currentSector = sectorName;
            Array.Resize(ref completedSectors, completedSectorsCount);
            completedSectors[completedSectorsCount - 1] = sectorName;
        }

    }

    IEnumerator RestartLastSector()
    {
        yield return new WaitForSeconds(restartLevelDelay);

        ObstaclesRoller or = obstacles.GetComponent<ObstaclesRoller>();

        foreach(GameObject go in or.transform.GetAllChildren())
        {
            float postionX = or.levelXPositions[completedSectorsCount - 1] + or.offsetX + or.startingX;

            or.transform.position += new Vector3(postionX, 0.0f, 0.0f);
        }
        
    }
}
