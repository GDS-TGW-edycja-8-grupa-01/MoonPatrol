using System;
using UnityEngine;
using PathCreation;
using ExtensionMethods;

public class EnemyMovement : MonoBehaviour
{
    private PathCreator pc;

    public float speed;
    private float distanceTravelled;
    private Vector2 direction;
    private Vector3 targetVector;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool onThePath;
    private bool kamikaze = false;
    private bool clockwise;
    private int boolInt;
    private GameManager gameManager;
    private Vector2 screen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        PreparePath();
    }
    void Start()
    {
        gameManager = this.gameObject.GetGameManager();
        screen = new Vector2(gameManager.GetScreenBounds().x, gameManager.GetScreenBounds().y);
        distanceTravelled = UnityEngine.Random.Range(0.0f, pc.path.length);
        transform.position = pc.path.GetPointAtDistance(distanceTravelled);
        onThePath = true;
        sr.enabled = true;
        boolInt = clockwise ? 1 : -1;
        return;
    }

    void Update()
    {
        if (onThePath)
        {
            MoveAlongThePath();
        }
        else if(kamikaze)
        {
            MoveToTarget();
        }
        else
        {
            MoveToTarget();
            if (transform.position.x < -screen.x - 1.0f) Destroy(this.gameObject);
        }
    }
    public void Decide(bool willToDie)
    { 
        kamikaze = willToDie;
        onThePath = false;
        targetVector = FindTargetVector();
    }
    private Vector3 FindTargetVector()
    {
        Vector3 target;
        GameObject player = GameObject.Find("Player");
        if (kamikaze && player)
        {
            Debug.Log("TORA TORA TORA");
            target = player.transform.position - transform.position;
            return target.normalized;
        }
        else
        {
            target = new Vector3(transform.position.x - screen.x * 2.0f, transform.position.y, 0.0f);
            return target.normalized;
        }
    }

    private void MoveToTarget()
    {
        Vector2 vector = new Vector2(targetVector.x, targetVector.y);
        transform.Translate(vector * speed * Time.deltaTime);
    }

    private void MoveAlongThePath()
    {
        //zbieram info o pozycji by móc wyrzucić informację o wektorze ruchu za pomocą GetDirection()
        Vector3 lastPosition = transform.position;
        distanceTravelled += speed * Time.deltaTime * boolInt;
        transform.position = pc.path.GetPointAtDistance(distanceTravelled);

        direction = (transform.position - lastPosition);
    }

    private void PreparePath()
    {
        var r = new System.Random();

        GameObject paths = GameObject.Find("EnemyPaths");

        int pathCount = paths.transform.childCount;
        string pathName = r.Next(1, pathCount + 1).ToString("00");

        GameObject path = GameObject.Find("EnemyPath" + pathName);
        PathCreator selectedPathCreator = path.GetComponent<PathCreator>();
        float factor = UnityEngine.Random.Range(0.0f, 1.0f);
        clockwise = factor <= 0.5f ? true : false;
        this.pc = selectedPathCreator;

        return;
    }
    
    public Vector2 GetDirection()
    {
        //zbyt małe wektory normalizowane są do [0,0]
        direction *= 100;
        return direction.normalized;
    }
}
