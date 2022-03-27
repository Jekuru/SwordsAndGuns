using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnerController : MonoBehaviour
{
    public GameObject weaponSpawner;
    private Weapon weaponController;
    private SpriteRenderer spriteRenderer;
    public float spawnTime;
    private float timer = 0.0f;

    private PhotonView photonView;

    // Armas
    public enum WeaponTypes // TIPOS DE ARMA, INTRODUCIR AQUÍ EL NOMBRE DE LAS NUEVAS ARMAS
    {
        none,                                       // Nada
        sword,                                      // Melee
        pistol, shotgun, sniper,                    // Distancia físicas
        raygun,                                     // Distancia raycast
                                                    // Proyectil AoE
    }
    // Arma equipada actualmente
    public WeaponTypes currentWeapon; // Arma actual

    public bool[] enabledWeaponsb = { false, true, true, true, true, true };

    public bool isActive = true;
    public bool startRandom; // Indica si el primer spawn del arma es aleatorio o no.

    public Sprite[] spriteList;

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
            WeaponTypes selectedWeapon;
            int randomNumber;
            do
            {
                randomNumber = Random.Range(1, System.Enum.GetValues(typeof(WeaponTypes)).Length);
                selectedWeapon = (WeaponTypes)randomNumber;
            } while (!enabledWeaponsb[randomNumber]);
            
            photonView.RPC("StartRandom", RpcTarget.All, selectedWeapon);
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
                WeaponTypes selectedWeapon;
                int randomNumber;
                do
                {
                    randomNumber = Random.Range(1, System.Enum.GetValues(typeof(WeaponTypes)).Length);
                    selectedWeapon = (WeaponTypes)randomNumber;
                } while (!enabledWeaponsb[randomNumber]);

                photonView.RPC("WeaponSpawner", RpcTarget.All, selectedWeapon);
            }
        }
    }

    [PunRPC]
    void StartRandom(WeaponTypes randomWeapon)
    {
        currentWeapon = randomWeapon;
        ChangeWeaponSprite();
    }

    [PunRPC]
    public void WeaponSpawner(WeaponTypes randomWeapon)
    {
        weaponSpawner.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        isActive = true;

        currentWeapon = randomWeapon;

        //Cambia el sprite del arma segun el arma elegida aleatoriamente
        ChangeWeaponSprite();
    }

    private void ChangeWeaponSprite()
    {
        switch (currentWeapon)
        {
            case WeaponTypes.sword:
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[0];
                break;

            case WeaponTypes.pistol:
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[1];
                break;

            case WeaponTypes.shotgun:
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[2];
                break;

            case WeaponTypes.sniper:
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[3];
                break;

            case WeaponTypes.raygun:
                weaponSpawner.GetComponent<SpriteRenderer>().sprite = spriteList[4];
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerStats>().isDead)
                return;

            weaponController = collision.gameObject.GetComponent<Weapon>();
            spriteRenderer = weaponController.weapon.GetComponent<SpriteRenderer>();

            if (collision.gameObject.GetComponent<Weapon>().currentWeapon == Weapon.WeaponTypes.none)
            {
                weaponController.WeaponChange((int)currentWeapon);
                photonView.RPC("PickedUp", RpcTarget.All);
                /*
                currentWeapon = WeaponTypes.none;
                weaponSpawner.SetActive(false);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                isActive = false;
                timer = 0f;^*/
            }
        }
    }

    [PunRPC]
    void PickedUp()
    {
        currentWeapon = WeaponTypes.none;
        weaponSpawner.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        isActive = false;
        timer = 0f;
    }
}
