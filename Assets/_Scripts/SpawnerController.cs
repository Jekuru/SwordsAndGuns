using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject weaponSpawner;
    public float spawnTime;
    private float timer = 0.0f;
    public string spawnedWeapon;
    public bool isActive = true;


    // Update is called once per frame
    void Update()
    {
        if (isActive == false)
        {
            Time.timeScale = 1.0f;
            timer += Time.deltaTime;
            if (timer > spawnTime)
            {
                weaponSpawner.SetActive(false);
                isActive = true;
            }

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Player"))
        {
            weaponSpawner.SetActive(false);
            isActive = false;
        }
    }

}
