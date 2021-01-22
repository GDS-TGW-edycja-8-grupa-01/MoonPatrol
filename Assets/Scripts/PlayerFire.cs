using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Range(0.1f, 3.0f)]
    public float shootDelay = 3.0f;

    public GameObject roofGun;
    public GameObject frontGun;

    public GameObject air2AirMissile;
    public GameObject air2SurfaceMissile;
    public PlayerSound playerAudioScript;

    private bool canShoot = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            Instantiate(air2AirMissile, roofGun.transform);
            playerAudioScript.WeaponShootUp();

            if (canShoot) {
                GameObject missile = Instantiate(air2SurfaceMissile, frontGun.transform);
                Physics2D.IgnoreCollision(missile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                playerAudioScript.WeaponShootDown();
                canShoot = false;
                StartCoroutine(ShootDelay());
            }
            

        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);

        canShoot = true;
    }
}
