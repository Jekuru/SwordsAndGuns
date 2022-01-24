using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]

public class Weapon : MonoBehaviour
{
    public Controller controller;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private void Awake()
    {
        controller = transform.parent.GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.input.RetrieveFireInput())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
