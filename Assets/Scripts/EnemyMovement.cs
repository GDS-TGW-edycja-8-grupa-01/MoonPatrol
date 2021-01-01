using System;
using UnityEngine;
using PathCreation;

public class EnemyMovement : MonoBehaviour
{
    private PathCreator pc;

    public float speed;
    private float distanceTravelled;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        PreparePath();

        return;
    }

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pc.path.GetPointAtDistance(distanceTravelled);
    }

    private void PreparePath()
    {
        var r = new System.Random();

        GameObject paths = GameObject.Find("EnemyPaths");

        int pathCount = paths.transform.childCount;
        string pathName = r.Next(1, pathCount + 1).ToString("00");

        GameObject path = GameObject.Find("EnemyPath" + pathName);
        PathCreator selectedPathCreator = path.GetComponent<PathCreator>();

        this.pc = selectedPathCreator;

        return;
    }
}
