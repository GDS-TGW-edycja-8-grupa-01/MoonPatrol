﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public MusicScript musicManagementScript;
    public AudioMixerSnapshot gameAudio;
    public AudioMixerSnapshot menuAudio;
    [Range(0.0f, 1.0f)]
    public float audioTransitionTime = 0.2f;
    public GameObject enemyContainer;
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

    [Range(0, 120)]
    public float averageTime = 60;

    [Range(1, 999)]
    public int livesCount;
    private int remaingingLivesCount;

    public bool godMode;

    public RectMask2D fillBar;
    public Text scoreText;
    public Text highScoreText;
    public Text remainingLiveText;
    public Text timeText;
    public Button playButton;
    public Button quitButton;
    public Image buttonPanel;
    public Image logoImage;
    public Image medalImage;
    public GameObject gameOverGroup;
    public GameObject mainMenuGroup;
    public GameObject stageSummaryGroup;
    public GameObject CreditsGroup;
    public GameObject HUDGroup;
    public Text greatJobText;
    public Text reachedPointText;
    public Text gameOverScoreText;
    public Text yourTimeText;
    public Text averageTimeText;
    public Text bonusPointsText;
    public Text totalScoreText;

    public int score = 0;
    public int highScore = 0;
    public int seconds = 0;
    public bool timerStarted = false;
    private float startTime;
    private string[] timerResetingSectors = { "f", "k", "p", "u", "z" };
    private string winningSector = "p";

    //private string[] timerResetingSectors = { "b", "k", "p", "u", "z" };
    //private string winningSector = "b";

    public int jumpedOverRockPoints = 50;
    public int destroyedRockPoints = 100;
    public int destroyedEnemyPoints = 100;
    public int bonusPointsPerSecond = 100;

    public static event EventHandler OnRestartSector;

    public int smallComboPoints = 400;
    public int bigComboPoints = 700;
    public float comboTime = 3.0f;
    private int comboCounter = 0;

    private float barValue = 0.0f;

    ObstaclesRoller or;

    public bool isPresentingSectorSummary = false;
    public bool isPresentingCredits = false;

    private int stageLandscapeIndex;

    GameObject playerGo;

    private void Start()
    {
        Time.timeScale = 0.0f;

        JumpCollider.OnJumpedOverObstacle += JumpCollider_OnJumpedOverObstacle;
        Rock.OnRockDestroyed += Rock_OnRockDestroyed;
        EnemyHit.OnEnemyDestroyed += EnemyHit_OnEnemyDestroyed;
        
        or = obstacles.GetComponent<ObstaclesRoller>();
    }

    public void EscapePressed()
    {
        Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;

        if (isPresentingSectorSummary)
        {
            HideSectorSummary();
        }
    }

    public void Play()
    {
        stageLandscapeIndex = -1;
        ChangeDistantLandscapeLayer();

        remaingingLivesCount = livesCount;

        RollbackAllLevels();
        gameAudio.TransitionTo(audioTransitionTime);
        musicManagementScript.SwitchMusic(false);
        ChangeBackgroundScrollSpeed(2.5f);
        playerGo = Instantiate(player);

        playerGo.GetComponent<PlayerHit>().godMode = godMode;

        Time.timeScale = 1.0f;

        score = 0;

        UpdateUI();

        gameOverGroup.SetActive(false);
        mainMenuGroup.SetActive(false);
        HUDGroup.SetActive(true);

        GameObject earthBase = GameObject.Find("Earthbase");
        earthBase.transform.position = new Vector2(-3.76f, -2);
    }

    private void RollbackAllLevels()
    {
        startedSectorsCount = 0;
        startedSectors = new string[] { };

        List<GameObject> allLevels = GetLevelsToRollback(or);
        RollbackLevels(allLevels);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        CreditsGroup.SetActive(true);

        isPresentingCredits = true;
    }

    private void Rock_OnRockDestroyed(object sender, EventArgs e)
    {
        score += destroyedRockPoints;
        highScore = score >= highScore ? score : highScore;

        UpdateUI();
    }

    private void EnemyHit_OnEnemyDestroyed(object sender, EventArgs e)
    {
        score += destroyedEnemyPoints;
        highScore = score >= highScore ? score : highScore;
        if (comboCounter == 0) StartCoroutine(ComboTimer());
        comboCounter++;
        UpdateUI();
    }

    private void JumpCollider_OnJumpedOverObstacle(object sender, EventArgs e)
    {
        JumpCollider jc = (JumpCollider)sender;
        Debug.Log("JUMPED OVER : " + jc.transform.parent.gameObject.name);
        score += jumpedOverRockPoints;
        highScore = score >= highScore ? score : highScore;

        UpdateUI();
    }
    private void ComboScore()
    {
        if(comboCounter >= 5)
        {
            score += bigComboPoints;
        }
        else if(comboCounter >= 3)
        {
            score += smallComboPoints;
        }
        UpdateUI();
    }
    private IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(comboTime);
        ComboScore();
        comboCounter = 0;
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString("000000");
        gameOverScoreText.text = score.ToString("000000");
        highScoreText.text = highScore.ToString("000000");
        remainingLiveText.text = remaingingLivesCount.ToString();
        timeText.text = seconds.ToString("000");
        fillBar.padding = new Vector4(0.0f, 0.0f, positionToLevelBar(), 0.0f);
    }

    private float positionToLevelBar()
    { 
        //I nagrodę za najbardziej naćkany kod wygrywa... *werble* ...funkcja liczenia paska postępu!
        GameObject playerGo = GameObject.Find("Player");
        
        if (playerGo != null)
        {
            ObstaclesRoller or = obstacles.GetComponent<ObstaclesRoller>();
            float positionA = Mathf.Clamp(or.transform.GetChild(currentSectorIndex(or)).transform.position.x, float.NegativeInfinity, 0.0f) - or.levelLength * currentSectorIndex(or);
            //Debug.Log("POSITION OF LEVEL : " + positionA.ToString());
            float distanceTravelled = playerGo.transform.position.x - positionA;
            float barWidth = fillBar.GetComponent<RectTransform>().rect.width;
            float completionFactor = Mathf.Clamp(distanceTravelled / (or.levelLength * or.levels.Length), 0.0f, 1.0f);
            barValue = barWidth - barWidth * completionFactor - 6.0f;
            return barValue;
        }
        else
        {
            return barValue;
        }
    }

    private int currentSectorIndex(ObstaclesRoller or)
    {
        //ObstaclesRoller or = obstacles.GetComponent<ObstaclesRoller>();
        return Mathf.Clamp(startedSectors.Length - 1, 0, or.levels.Length);
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
        
        Physics2D.IgnoreLayerCollision(10, 12, false);

        if (remaingingLivesCount < 1)
        {
            GameOver();

            return;
        }
        ChangeBackgroundScrollSpeed(2.5f);
        Vector3 respawn = GetRespawn();
        
        playerGo = Instantiate(player, respawn, Quaternion.identity);

        playerGo.GetComponent<PlayerHit>().WheelsSetActive(true);
        playerGo.transform.Find("Explosion").gameObject.GetComponent<Animator>().enabled = false;
    }

    private Vector3 GetRespawn()
    {
        GameObject respawn;

        respawn = GameObject.Find("Spawn");

        return respawn.transform.position;
    }

    private void GameOver()
    {
        timerStarted = false;

        gameOverGroup.SetActive(true);
        StartCoroutine(ShowMainMenu());

        UpdateUI();
        menuAudio.TransitionTo(audioTransitionTime);

        return;
    }

    IEnumerator ShowMainMenu()
    {
        yield return new WaitForSeconds(2);
        
        playButton.GetComponentInChildren<Text>().text = "New Game";

        mainMenuGroup.SetActive(true);
        HUDGroup.SetActive(false);
        gameOverGroup.SetActive(false);
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
                
                DisplaySectorSummary();

                ChangeDistantLandscapeLayer();

                return;
            }

            return;
        }

        return;
    }

    private void ChangeDistantLandscapeLayer()
    {
        stageLandscapeIndex = stageLandscapeIndex == 2 ? 0 : ++stageLandscapeIndex;
        
        GameObject hills = background.transform.GetAllChildren().First<GameObject>(go => go.name == "Hills");
        GameObject suburbs = background.transform.GetAllChildren().First<GameObject>(go => go.name == "Suburbs");
        GameObject city = background.transform.GetAllChildren().First<GameObject>(go => go.name == "City");

        hills.SetActive(stageLandscapeIndex == 0);
        suburbs.SetActive(stageLandscapeIndex == 1);
        city.SetActive(stageLandscapeIndex == 2);

        return;
    }

    private void DisplaySectorSummary()
    {
        menuAudio.TransitionTo(audioTransitionTime);
        Time.timeScale = 0.0f;
        ClearEnemies();
        isPresentingSectorSummary = true;
        string reachedPointTextValue;
        int bonusPoints;

        if (currentSector == winningSector)
        {
            
            reachedPointTextValue = "Last point reached!";
            greatJobText.text = "YOU WON!";
        }
        else
        {
            greatJobText.text = "GREAT JOB!";
            reachedPointTextValue = $"Point \"{currentSector.ToUpper()}\" reached!";
        }

        reachedPointText.text = reachedPointTextValue;
        yourTimeText.text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
        averageTimeText.text = TimeSpan.FromSeconds(averageTime).ToString(@"mm\:ss");
        medalImage.gameObject.SetActive(currentSector == winningSector);

        bonusPoints = (int)(averageTime - seconds) * bonusPointsPerSecond;
        bonusPointsText.text = bonusPoints > 0 ? $"{bonusPoints}" : "0";

        score += bonusPoints > 0 ? bonusPoints : 0;
        highScore = score >= highScore ? score : highScore;
        totalScoreText.text = score.ToString();

        stageSummaryGroup.SetActive(true);
    }

    public void HideSectorSummary()
    {
        if (currentSector != winningSector)
        {
            Time.timeScale = 1.0f;

            gameAudio.TransitionTo(audioTransitionTime);

            isPresentingSectorSummary = false;
            stageSummaryGroup.SetActive(false);

            return;
        }

        GameOverAfterLastPoint();
    }

    private void GameOverAfterLastPoint()
    {
        Destroy(playerGo);

        isPresentingSectorSummary = false;
        stageSummaryGroup.SetActive(false);

        playButton.GetComponentInChildren<Text>().text = "New Game";

        mainMenuGroup.SetActive(true);
        HUDGroup.SetActive(false);

        GameObject earthBase = GameObject.Find("Earthbase");
        earthBase.transform.position = new Vector2(-3.76f, -2);

        OnRestartSector?.Invoke(this, EventArgs.Empty);
        ClearEnemies();

        return;
    }

    IEnumerator RestartLastSector()
    {
        yield return new WaitForSeconds(restartLevelDelay);
                
        List<GameObject> levelsToComplete = GetLevelsToRollback(or);
        RollbackLevels(levelsToComplete);

        OnRestartSector?.Invoke(this, EventArgs.Empty);
        ClearEnemies();
        Respawn();
    }

    private void RollbackLevels(List<GameObject> levelsToRollback)
    {
        float y = -4.18f;
        float z = -2.0f;
        float restartXPosition = 8.0f;
        float previousLevelLength = restartXPosition;

        foreach (GameObject level in levelsToRollback)
        {
            level.transform.position = new Vector3(previousLevelLength, y, z);

            previousLevelLength += or.levelLength;
        }
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

    private void ClearEnemies()
    {
        for(int i=0; i < enemyContainer.transform.childCount; i++)
        {
            Destroy(enemyContainer.transform.GetChild(i).gameObject);
        }
        List<GameObject> holeContainer = obstacles.transform.GetChild(0).GetAllChildren();
        foreach (GameObject obj in holeContainer)
        {
            if (obj.name == "DESTROY ME") Destroy(obj);
        }
        
    }

    private void Update()
    {
        if (!timerStarted)
        {
            startTime = Time.time;
        }

        seconds = timerStarted ? (int)((Time.time - startTime)) : 0;

        UpdateUI();

        if (Input.anyKeyDown && isPresentingCredits) CreditsGroup.SetActive(false);
    }
}
