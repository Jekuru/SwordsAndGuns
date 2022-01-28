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

    public GameObject spreadArea;

    private void Awake()
    {
        controller = GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.input.RetrieveFireInput())
        {
            switch (currentWeapon)
            {
                case WeaponTypes.none:
                    break;
                case WeaponTypes.pistol:
                    Shoot(firePoint.position, firePoint.rotation); // TODO
                    break;
                case WeaponTypes.shotgun:
                    Shotgun();
                    break;
                default:
                    break;
            }
        }
    }

    void Shotgun()
    {
        int pellets = 10;
        for (int i = 0; i < pellets; i++)
        {
            float randomBullet = Random.Range(-30f, 30f);
            Quaternion newRotation = firePoint.rotation;
            newRotation = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);

            Shoot(firePoint.position, newRotation);
        }
    }

    void Shoot(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletPrefab, position, rotation);
    }
}
