using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [HideInInspector]
    public float scrollSpeed;
    [Range(0.0f, 1.0f)]
    public float scrollMod = 0.52f;
    private Material mat;
    private float offset = 0f;

    public Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed = 2.5f;

        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) * scrollMod / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }

    public void ApplyTexture(int matIndex)
    {
        mat = materials[matIndex];
    }
}
