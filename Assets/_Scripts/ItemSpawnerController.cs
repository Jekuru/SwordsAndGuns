using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerController : MonoBehaviour
{
    public GameObject spawner;
    private PlayerStats playerStats;
    private Jump jump;
    private SpriteRenderer spriteRenderer;
    public float spawnTime;
    private float timer = 0.0f;
    // Armas
    public enum ItemTypes // TIPOS DE OBJETOS, INTRODUCIR AQUÍ EL NOMBRE DE LOS NUEVOS OBJETOS
    {
        shield,
        boots,
    }

    public ItemTypes currentItem; // Arma actual

    public bool isActive = true;
    public bool startRandom; // Indica si el primer spawn del objeto es aleatorio o no.

    public Sprite[] spriteList; // Array de sprites

    private void Start()
    {
        if (isActive && startRandom)
        {
            currentItem = (ItemTypes)Random.Range(0, System.Enum.GetValues(typeof(ItemTypes)).Length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vuelve a mostrar un arma aleaoria entre las dadas tras un tiempo

        if (isActive == false)
        {
            Time.timeScale = 1.0f;
            timer += Time.deltaTime;
            if (timer > spawnTime)
            {
                spawner.SetActive(true);
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                isActive = true;

                currentItem = (ItemTypes)Random.Range(1, System.Enum.GetValues(typeof(ItemTypes)).Length);
            }

        }

        //Cambia el sprite del arma segun el arma elegida aleatoriamente

        switch (currentItem)
        {
            case ItemTypes.shield:
                spawner.GetComponent<SpriteRenderer>().sprite = spriteList[0];
                break;

            case ItemTypes.boots:
                spawner.GetComponent<SpriteRenderer>().sprite = spriteList[1];
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats.isDead)
                return;

            jump = collision.gameObject.GetComponent<Jump>();
            if(currentItem == ItemTypes.shield && playerStats.healthPoints == 1)
            {
                playerStats.healthPoints = 2;
            }
            else if(currentItem == ItemTypes.boots && jump.maxAirJumps == 0)
            {
                jump.maxAirJumps = 1;
            } 
            else
            {
                return;
            }

            spawner.SetActive(false);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isActive = false;
            timer = 0f;
            }
        }
    }