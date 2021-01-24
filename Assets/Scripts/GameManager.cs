using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Linq;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject background;
    private GameObject backgroundScroller;

    public GameObject ground;
    public GameObject obstacles;
    private GameObject groundScroller;

    public GameObject player;
    public GameObject playerRagdoll;

    public string currentSector = "a";
    public int startedSectorsCount = 0;
    public string[] startedSectors;

    [Range(0.0f, 10.0f)]
    public float restartLevelDelay = 2.0f;

    [Range(1, 999)]
    public int remaingingLivesCount = 1;

    public bool godMode;

    public Text scoreText;
    public Text highScoreText;
    public Text remainingLiveText;
    public Text timeText;
    public Button playButton;
    public Button quitButton;

    public int score = 0;
    public int highScore = 0;
    public int seconds = 0;
    public bool timerStarted = false;
    private float startTime;
    private string[] timerResetingSectors = { "b", "c" };

    [Range(50, 60)]
    public int jumpedOverRockPoints = 50;
    public int destroyedRockPoints = 50;

    public static event EventHandler OnRestartSector;

    private void Start()
    {
        Time.timeScale = 0.0f;

        JumpCollider.OnJumpedOverObstacle += JumpCollider_OnJumpedOverObstacle;
        Rock.OnRockDestroyed += Rock_OnRockDestroyed;
    }

    public void EscapePressed()
    {
        Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
    }

    public void Play()
    {
        GameObject playerGo = Instantiate(player);

        playerGo.GetComponent<PlayerHit>().godMode = godMode;

        Time.timeScale = 1.0f;

        UpdateUI();

        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Rock_OnRockDestroyed(object sender, EventArgs e)
    {
        score += destroyedRockPoints;
        highScore = score <= highScore ? score : highScore;

        UpdateUI();
    }

    private void JumpCollider_OnJumpedOverObstacle(object sender, EventArgs e)
    {
        score += jumpedOverRockPoints;
        highScore = score <= highScore ? score : highScore;

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString("000000");
        highScoreText.text = highScore.ToString("000000");
        remainingLiveText.text = remaingingLivesCount.ToString();
        timeText.text = seconds.ToString("00");
    }

    public void Die()
    {
        ChangeBackgroundScrollSpeed(0.0f);
        ChangeRollingScrollSpeed(ground, 0.0f);
        ChangeRollingScrollSpeed(obstacles, 0.0f);

        GameObject playerGo = GameObject.Find("Player").gameObject;

        GameObject ragdoll = Instantiate(playerRagdoll, playerGo.transform.position, transform.rotation);
        Destroy(ragdoll, restartLevelDelay);

        if (playerGo != null)
        {
            Destroy(playerGo);
            //Destroy(playerGo.transform.Find("Explosion").gameObject, restartLevelDelay);
        }

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
        remaingingLivesCount--;
        UpdateUI();

        if (remaingingLivesCount < 1)
        {
            GameOver();

            return;
        }

        Vector3 respawn = GetRespawn();
        Physics2D.IgnoreLayerCollision(10, 12, false);
        GameObject go = Instantiate(player, respawn, Quaternion.identity);
        go.GetComponent<PlayerHit>().WheelsSetActive(true);
        go.transform.Find("Explosion").gameObject.GetComponent<Animator>().enabled = false;
    }

    private Vector3 GetRespawn()
    {
        GameObject respawn;

        respawn = GameObject.Find("Spawn");

        return respawn.transform.position;
    }

    private void GameOver()
    {
        return;
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

            if (startedSectorsCount == 1)
            {
                timerStarted = true;
            }

            if (timerResetingSectors.Contains(sectorName))
            {
                startTime = Time.time;
            }

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

        OnRestartSector?.Invoke(this, EventArgs.Empty);

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

    private void Update()
    {
        if (!timerStarted)
        {
            startTime = Time.time;
        }

        seconds = timerStarted ? (int)((Time.time - startTime) % 60f) : 0;

        UpdateUI();
    }
}
