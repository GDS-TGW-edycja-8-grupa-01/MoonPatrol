using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Linq;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    public GameObject gameOverGroup;
    public GameObject mainMenuGroup;
    public GameObject stageSummaryGroup;
    public Text reachedPointText;
    public Text gameOverScoreText;
    public Text yourTimeText;
    public Text averageTimeText;

    public int score = 0;
    public int highScore = 0;
    public int seconds = 0;
    public bool timerStarted = false;
    private float startTime;
    private string[] timerResetingSectors = { "b", "c" };

    public int jumpedOverRockPoints = 50;
    public int destroyedRockPoints = 100;
    public int destroyedEnemyPoints = 100;

    public static event EventHandler OnRestartSector;

    public int smallComboPoints = 400;
    public int bigComboPoints = 700;
    public float comboTime = 3.0f;
    private int comboCounter = 0;

    private float barValue = 0.0f;

    ObstaclesRoller or;

    public bool isPresentingSectorSummary = false;

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
        remaingingLivesCount = livesCount;

        RollbackAllLevels();
        
        ChangeBackgroundScrollSpeed(2.5f);
        GameObject playerGo = Instantiate(player);

        playerGo.GetComponent<PlayerHit>().godMode = godMode;

        Time.timeScale = 1.0f;

        score = 0;

        UpdateUI();

        gameOverGroup.SetActive(false);
        mainMenuGroup.SetActive(false);
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
        timerStarted = false;

        gameOverGroup.SetActive(true);
        StartCoroutine(ShowUIGroup());

        UpdateUI();

        return;
    }

    IEnumerator ShowUIGroup()
    {
        yield return new WaitForSeconds(2);

        GameObject logoImage = mainMenuGroup.transform.Find("LogoImage").gameObject;

        playButton.GetComponentInChildren<Text>().text = "NEW GAME";

        logoImage.SetActive(false);
        mainMenuGroup.SetActive(true);
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
            }

            return;
        }

        return;
    }

    private void DisplaySectorSummary()
    {
        Time.timeScale = 0.0f;

        isPresentingSectorSummary = true;

        reachedPointText.text = $"Point \"{currentSector.ToUpper()}\" reached!";
        yourTimeText.text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
        averageTimeText.text = TimeSpan.FromSeconds(averageTime).ToString(@"mm\:ss");

        stageSummaryGroup.SetActive(true);
    }

    public void HideSectorSummary()
    {
        Time.timeScale = 1.0f;

        isPresentingSectorSummary = false;
        stageSummaryGroup.SetActive(false);
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

        seconds = timerStarted ? (int)((Time.time - startTime) % 60f) : 0;

        UpdateUI();
    }
}
