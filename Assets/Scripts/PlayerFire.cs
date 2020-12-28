using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject roofGun;
    public GameObject frontGun;

    public GameObject air2AirMissile;
    public GameObject air2SurfaceMissile;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            Instantiate(air2AirMissile, roofGun.transform);
            Instantiate(air2SurfaceMissile, frontGun.transform);

        }
    }
}
