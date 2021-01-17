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

    [Range(40, 220)]
    public int levelLength = 80;

    public float[] levelXPositions = { };

    [Range(0, 40)]
    public int initialXPosition = 30;

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

        float previousLevelLength = 0.0f;
        Vector3 position;
        
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == 0)
            {
                position = new Vector3(initialXPosition, y, z);
                previousLevelLength = position.x;// + initialXPosition;
            }
            else
            {
                previousLevelLength += levelLength;
                position = new Vector3(previousLevelLength, y, z);
            }

            GameObject go = Instantiate(levels[i], position, Quaternion.identity) as GameObject;
            go.transform.parent = transform;

            levelXPositions[i] = position.x;
        }

        return;
    }
}
