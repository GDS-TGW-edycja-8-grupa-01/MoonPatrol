using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [HideInInspector]
    public float scrollSpeed = 2.5f;
    [Range(0.0f, 1.0f)]
    public float scrollMod = 0.52f;
    private Material mat;
    private float offset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) * scrollMod / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
