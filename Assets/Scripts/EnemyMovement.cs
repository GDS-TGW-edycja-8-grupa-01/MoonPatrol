using System;
using UnityEngine;
using PathCreation;

public class EnemyMovement : MonoBehaviour
{
    private PathCreator pc;

    public float speed;
    private float distanceTravelled;
    private Vector2 direction;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool clockwise;
    private int boolInt;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        PreparePath();
    }
    void Start()
    {
        distanceTravelled = UnityEngine.Random.Range(0.0f, pc.path.length);
        transform.position = pc.path.GetPointAtDistance(distanceTravelled);
        sr.enabled = true;
        boolInt = clockwise ? 1 : -1;
        return;
    }

    void Update()
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
