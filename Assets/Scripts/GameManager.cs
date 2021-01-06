using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class GameManager : MonoBehaviour
{
    public GameObject background;
    private GameObject backgroundScroller;

    public GameObject ground;
    private GameObject groundScroller;

    private void Start()
    {
        return;
    }

    public void Die()
    {
        ChangeBackgroundScrollSpeed(0.0f);
        ChangeGroundScrollSpeed(0.0f);
    }

    private void ChangeBackgroundScrollSpeed(float speed)
    {
        foreach (GameObject go in background.transform.GetAllChildren())
        {
            BackgroundScroller bs = go.GetComponent<BackgroundScroller>();

            bs.scrollSpeed = speed;
        }

    }

    private void ChangeGroundScrollSpeed(float speed)
    {
        foreach (GameObject go in ground.transform.GetAllChildren())
        {
            GroundScroller gs = go.GetComponent<GroundScroller>();

            gs.ChangeScrollSpeed(0.0f);
        }

    }
}
