using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Reflection;
using System.Linq;

public class ObstaclesRoller : MonoBehaviour
{
    [SerializeField]
    public GameObject[] levels;

    private Vector3 bounds;
    private Renderer r;

    //[Range(0.0f, 20.0f)]
    //public float offsetX = 1.0f;

    //[Range(0.0f, 20.0f)]
    //public float startingX = 10.0f;

    public float[] levelXPositions = { };

    void Start()
    {
        bounds = this.GetScreenBounds();

        levelXPositions = new float[levels.Length];
        LayoutLevels();

        return;
    }

    private void LayoutLevels()
    {
        float y = -4.18f;
        float z = -2.0f;

        float initialXPosition = GetLevelWidth(levels[0]) / 2.0f;
        float previousLevelLength = GetLevelWidth(levels[0]) - bounds.x * 2.0f;
        float levelWidth;
        Vector3 position;
        
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == 0)
            {
                position = new Vector3(initialXPosition, y, z);
                previousLevelLength = position.x;
            }
            else
            {
                levelWidth = GetLevelWidth(levels[i]);
                previousLevelLength += levelWidth;
                position = new Vector3(previousLevelLength, y, z);
                
            }

            GameObject go = Instantiate(levels[i], position, Quaternion.identity) as GameObject;
            go.transform.parent = transform;

            //go.transform.position = position;
            levelXPositions[i] = position.x;
        }

        return;
    }

    private float GetLevelWidth(GameObject go)
    {
        return 80.0f;
    }
}
