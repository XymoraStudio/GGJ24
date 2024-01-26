using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header ("Gun")]
    [SerializeField] private float firerate; // Bullets per second

    [Header ("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float bulletSpeed;

    private State state;
    private float ammo;
    private float lastShootingTime = 0;

    enum State
    {
        Shooting,
        Idle
    }

    void Start()
    {
        state = State.Idle;
        ammo = 100; // DEBUG ONLY !
    }

    void Update()
    {
        // We update gun's state
        if (Input.GetKeyDown(KeyBinds.instance.gunShoot))
        {
            if (state == State.Idle) state = State.Shooting;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (state == State.Shooting) state = State.Idle;
        }

        // We use gun's state for decisions
        if (state == State.Shooting)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time - lastShootingTime > 1 / firerate) // Enough time have passed to shoot again
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().Initialize(bulletDamage, bulletSpeed, bulletLifetime);
            lastShootingTime = Time.time;
        }
    }
}
