using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpinningWheels : MonoBehaviour
{
    public GameObject frontWheel, rearWheel;
    [Range(360.0f, 1080.0f)]
    public float rotationSpeed = 360.0f;

    void Update()
    {
        Vector3 rotation = new Vector3(0, 0, -rotationSpeed * Time.deltaTime);
        frontWheel.transform.Rotate(rotation);
        rearWheel.transform.Rotate(rotation);
    }
}
