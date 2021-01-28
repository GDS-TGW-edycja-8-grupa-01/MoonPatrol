using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Range(0.1f, 3.0f)]
    public float bigShootDelay = 3.0f;
    [Range(0.1f, 3.0f)]
    public float topShootDelay = 3.0f;

    public GameObject roofGun;
    public GameObject frontGun;

    public GameObject air2AirMissile;
    public GameObject air2SurfaceMissile;
    public PlayerSound playerAudioScript;

    private bool bigCanShoot = true;
    private bool topCanShoot = true;
    private int topshotCounter;

    void Start()
    {
        topshotCounter = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            if (topCanShoot)
            {
                Instantiate(air2AirMissile, roofGun.transform.position, Quaternion.identity);
                playerAudioScript.WeaponShootUp();
                if (topshotCounter == 0) StartCoroutine(TopShootCounterClear());
                topshotCounter++;
            }

            if (bigCanShoot) {
                GameObject missile = Instantiate(air2SurfaceMissile, frontGun.transform);
                Physics2D.IgnoreCollision(missile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                playerAudioScript.WeaponShootDown();
                bigCanShoot = false;
                StartCoroutine(BigShootDelay());
            }
            

        }

        if ((topshotCounter >= 4) && topCanShoot)
        {
            topCanShoot = false;
            StartCoroutine(TopShootDelay());
        }
    }

    IEnumerator BigShootDelay()
    {
        yield return new WaitForSeconds(bigShootDelay);

        bigCanShoot = true;
    }
    IEnumerator TopShootDelay()
    {
        yield return new WaitForSeconds(topShootDelay);
        topshotCounter = 0;
        topCanShoot = true;
    }

    IEnumerator TopShootCounterClear()
    {
        yield return new WaitForSeconds(1.0f);
        if (topshotCounter < 4) topshotCounter = 0;
    }    
}
