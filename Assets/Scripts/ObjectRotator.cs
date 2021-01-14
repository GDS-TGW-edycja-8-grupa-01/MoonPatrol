using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float anglePerSec = 120.0f;
    [Range(1.0f, 5.0f)]
    public float modSpeed = 3.0f;
    private float rotationMod;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rotationMod = Mathf.Sin(Mathf.Sin(Time.time * modSpeed)*Mathf.PI);
        rotationMod = Mathf.Sin(Time.time * modSpeed);
        //rotationMod = 1;
        this.transform.Rotate(0, 0, anglePerSec * Time.deltaTime * rotationMod, Space.Self);
    }
}

