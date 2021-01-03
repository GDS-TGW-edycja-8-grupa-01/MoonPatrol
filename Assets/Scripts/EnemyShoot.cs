using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    //Absolutny klon PlayerFire, z prostszą funkcjonalnościa.
    public GameObject missile;
    public int ammo = 2;
    [Range(0.1f,10.0f)]
    public float shootDelay = 1.0f;
    //Offset niezaimplementowany
    [Range(0.1f, 10.0f)]
    public float shootOffset = 1.0f;
    private bool canShoot = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot && ammo > 0)
        {
            ammo--;
            GameObject missileContainer = Instantiate(missile, transform.position, Quaternion.identity);
            //Pewnie da się to zrobić mądrzej, ale...
            Physics2D.IgnoreCollision(missileContainer.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            canShoot = false;
            StartCoroutine(ShootDelay());
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);

        canShoot = true;
    }
}
