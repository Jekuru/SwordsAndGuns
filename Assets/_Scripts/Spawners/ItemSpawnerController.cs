using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawnerController : MonoBehaviour
{
    public GameObject itemSpawner;
    private PlayerStats playerStats;
    private Jump jump;
    private SpriteRenderer spriteRenderer;
    public float spawnTime;
    private float timer = 0.0f;

    private PhotonView photonView;

    // Armas
    public enum ItemTypes // TIPOS DE OBJETOS, INTRODUCIR AQUÍ EL NOMBRE DE LOS NUEVOS OBJETOS
    {
        shield,
        boots,
    }

    public ItemTypes currentItem; // Arma actual

    public bool[] enabledItems = { true, true };

    public bool isActive = true;
    public bool startRandom; // Indica si el primer spawn del objeto es aleatorio o no.

    public Sprite[] spriteList; // Array de sprites

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient && !photonView.IsMine)
            return;

        if (isActive && startRandom)
        {
            ItemTypes selectedItem;
            int randomNumber;
            do
            {
                randomNumber = Random.Range(0, System.Enum.GetValues(typeof(ItemTypes)).Length);
                selectedItem = (ItemTypes)randomNumber;
            } while (!enabledItems[randomNumber]);

            photonView.RPC("StartRandomItem", RpcTarget.All, selectedItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient && !photonView.IsMine)
            return;
        //Vuelve a mostrar un arma aleaoria entre las dadas tras un tiempo

        if (isActive == false)
        {
            Time.timeScale = 1.0f;
            timer += Time.deltaTime;
            if (timer > spawnTime)
            {
                ItemTypes selectedItem;
                int randomNumber;
                do
                {
                    randomNumber = Random.Range(0, System.Enum.GetValues(typeof(ItemTypes)).Length);
                    selectedItem = (ItemTypes)randomNumber;
                } while (!enabledItems[randomNumber]);

                photonView.RPC("StartRandomItem", RpcTarget.All, selectedItem);
            }

        }
    }

    [PunRPC]
    void StartRandomItem(ItemTypes randomItem)
    {
        currentItem = randomItem;
        // Cambia el sprite según el objeto inicial elegido de forma aleatoria
        ChangeItemSprite();
    }

    [PunRPC]
    public void ItemSpawner(ItemTypes randomItem)
    {
        itemSpawner.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        isActive = true;

        currentItem = randomItem;

        // Cambia el sprite según el nuevo objeto
        ChangeItemSprite();
    }

    private void ChangeItemSprite()
    {
        switch (currentItem)
        {
            case ItemTypes.shield:
                itemSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[0];
                break;

            case ItemTypes.boots:
                itemSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[1];
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

            photonView.RPC("PickedUpItem", RpcTarget.All);
        }
    }

    [PunRPC]
    void PickedUpItem()
    {
        itemSpawner.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        isActive = false;
        timer = 0f;
    }
}