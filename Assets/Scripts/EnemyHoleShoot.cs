﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHoleShoot : MonoBehaviour
{
    //Absolutny klon PlayerFire, z prostszą funkcjonalnościa.
    public EnemyHoleMissile missile;
    public int ammo = 1;
    [Range(0.1f,10.0f)]
    public float shootDelay = 1.0f;
    //Offset niezaimplementowany
    [Range(0.1f, 10.0f)]
    public float shootOffset = 1.0f;
    private bool canShoot = true;
    private EnemyMovement em;
    private bool died = false;
    private bool ready = false;
    Transform gun;

    void Start()
    {
        em = GetComponent<EnemyMovement>();
        gun = transform.Find("Gun").gameObject.transform;
        StartCoroutine(ShootOffset());
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot && ammo > 0 && ready)
        {
            missile.direction = em.GetDirection();
            ammo--;
            EnemyHoleMissile missileContainer = Instantiate(missile, gun.position, Quaternion.identity);
            missileContainer.direction = em.GetDirection();
            //Pewnie da się to zrobić mądrzej, ale...
            Physics2D.IgnoreCollision(missileContainer.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            canShoot = false;
            StartCoroutine(ShootDelay());
        }
    }
    IEnumerator ShootOffset()
    {
        yield return new WaitForSeconds(shootOffset);

        ready = true;
        StopCoroutine(ShootOffset());
    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);

        canShoot = !died;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            died = true;
        }
    }
}