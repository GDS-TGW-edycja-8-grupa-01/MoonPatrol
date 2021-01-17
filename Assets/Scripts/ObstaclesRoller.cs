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

        Vector3 initialPosition = new Vector3(0.0f, y, z);
        float previousLevelLength = GetLevelWidth(levels[0]);
        Vector3 position;
        
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == 0)
            {
                position = initialPosition;
            }
            else
            {
                position = new Vector3(previousLevelLength, y, z);
                previousLevelLength += GetLevelWidth(levels[i]);
            }

            Instantiate(levels[i], transform, true);

            levels[i].transform.position = position;
            levelXPositions[i] = position.x;
        }

        return;
    }

    private float GetLevelWidth(GameObject go)
    {
        BoxCollider2D bc = go.GetComponent<BoxCollider2D>();
        Vector2 size = bc.size;
        float width = size.x;

        return width;
    }
}
