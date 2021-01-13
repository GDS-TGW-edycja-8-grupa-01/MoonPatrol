using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    [Range(0.1f, 3.0f)]
    public float lowEnginePitch = 0.9f;
    [Range(0.1f, 3.0f)]
    public float hiEnginePitch = 1.5f;
    private PlayerBounds pb;
    private Vector2 minMaxBounds;
    private float pitchFactor;
    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBounds>();
        minMaxBounds = pb.GetBounds();
    }

    // Update is called once per frame
    void Update()
    {
        pitchFactor = Mathf.Abs(transform.position.x - minMaxBounds.x) / Mathf.Abs(minMaxBounds.x - minMaxBounds.y);
        audioSource.pitch = Mathf.Lerp(lowEnginePitch, hiEnginePitch, Mathf.InverseLerp(0.0f, 1.0f, pitchFactor));
        //audioSource.pitch = pitchFactor;
        //Debug.Log("PITCHFACTOR : " + pitchFactor.ToString());
    }
}
