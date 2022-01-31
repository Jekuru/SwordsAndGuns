using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]

public class Weapon : MonoBehaviour
{
    private Controller controller;
    public Transform firePoint;
    public GameObject bulletPrefab;

    public enum WeaponTypes
    {
        none, pistol, shotgun
    }
    public WeaponTypes currentWeapon;

    public float spreadMin;
    public float spreadMax;

    public float fireRate;
    public float cdShoot = 0f;
    public bool cdBool;

    private void Awake()
    {
        controller = GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.input.RetrieveFireInput() && cdBool)
        {
            cdBool = false;
            switch (currentWeapon)
            {
                case WeaponTypes.none:
                    None();
                    break;
                case WeaponTypes.pistol:
                    Pistol();
                    break;
                case WeaponTypes.shotgun:
                    Shotgun();
                    break;
                default:
                    break;
            }
        }

        if (!cdBool)
            cdShoot += Time.deltaTime;

        if(cdShoot > fireRate)
        {
            cdShoot = 0f;
            cdBool = true;
        }
    }

    void None()
    {
        spreadMin = 0;
        spreadMax = 0;
        fireRate = 0;
    }

    void Pistol()
    {
        spreadMin = -2.5f;
        spreadMax = 2.5f;
        fireRate = 0.5f;

        float randomBullet = Random.Range(spreadMin, spreadMax);
        Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);
        Shoot(firePoint.position, firePoint.rotation * spread);
    }

    void Shotgun()
    {
        int pellets = 10;
        spreadMin = -30f;
        spreadMax = 30f;
        fireRate = 1f;

        for (int i = 0; i < pellets; i++)
        {
            float randomBullet = Random.Range(spreadMin, spreadMax);
            Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);

            Shoot(firePoint.position, firePoint.rotation * spread);
        }
    }

    void Shoot(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletPrefab, position, rotation);
    }
}
