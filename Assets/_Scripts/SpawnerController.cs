using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject weaponSpawner;
    public float spawnTime;
    private float timer = 0.0f;
    public string spawnedWeapon;
    public string[] avaliableWeapons;
    public bool isActive = true;
    int index;
    public Sprite[] spriteList;



    // Update is called once per frame
    void Update()
    {
        if (isActive == false)
        {
            Time.timeScale = 1.0f;
            timer += Time.deltaTime;
            if (timer > spawnTime)
            {
                weaponSpawner.SetActive(true);
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                isActive = true;

                index = Random.Range(0, avaliableWeapons.Length);
                spawnedWeapon = avaliableWeapons[index];
            }

        }

        switch (spawnedWeapon)
        {
            case "sword":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[0];
                break;

            case "dagger":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[1];
                break;

            case "greatsword":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[2];
                break;

            case "halberd":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[3];
                break;

            case "pistol":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[4];
                break;

            case "shotgun":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[5];
                break;

            case "rifle":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[6];
                break;

            case "sniper":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[7];
                break;

            case "rpg":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[8];
                break;

            case "raygun":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[9];
                break;

            case "grenade":
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[10];
                break;




        }




    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Player"))
        {
            weaponSpawner.SetActive(false);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isActive = false;
            timer = 0f;
        }
    }

}
