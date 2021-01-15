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

    void Start()
    {
        bounds = this.GetScreenBounds();

        LayoutLevels();

        return;
    }

    private void LayoutLevels()
    {
        Vector3 initialPosition = new Vector3(20.0f, -4.18f, -2.0f);
        float previousLevelLength = 50.0f;

        for (int i = 0; i < levels.GetLength(0); i++)
        {
            Vector3 position = i == 0 ? initialPosition : new Vector3(previousLevelLength, -4.18f, -2.0f);

            Instantiate(levels[i], transform, true);

            levels[i].transform.position = position;
        }

        return;
    }
}
