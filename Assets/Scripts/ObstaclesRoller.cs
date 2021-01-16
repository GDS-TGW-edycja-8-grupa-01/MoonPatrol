using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Reflection;

public class ObstaclesRoller : MonoBehaviour
{
    [SerializeField]
    public GameObject[] levels;

    private Vector3 bounds;
    private Renderer r;

    [Range(0.0f, 20.0f)]
    public float offsetX = 1.0f;

    void Start()
    {
        bounds = this.GetScreenBounds();

        LayoutLevels();

        return;
    }

    private void LayoutLevels()
    {
        float y = -4.18f;
        float z = -2.0f;
        float startingX = 10.0f;
        Vector3 initialPosition = new Vector3(startingX, y, z);
        float previousLevelLength = GetLevelWidth(levels[0]);
        Vector3 position;

        for (int i = 0; i < levels.GetLength(0); i++)
        {
            if (i == 0)
            {
                position = initialPosition;
            }
            else
            {
                position = new Vector3(startingX + previousLevelLength + offsetX, y, z);
                previousLevelLength += GetLevelWidth(levels[i]);
            }

            Instantiate(levels[i], transform, true);

            levels[i].transform.position = position;
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
