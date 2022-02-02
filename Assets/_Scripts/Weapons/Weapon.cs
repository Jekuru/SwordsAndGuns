using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]

public class Weapon : MonoBehaviour
{
    private Controller controller;
    public Transform firePoint; // Desde donde sale el proyectil
    public GameObject weapon; // GameObject del arma
    public GameObject bulletPrefab; // Proyectil para las armas a distancia que utlizan físicas
    public SpriteRenderer currentWeaponSprite; // Sprite actual del arma equipada
    public Sprite[] spriteArray; // Array de sprites, añadir desde el inspector

    public enum WeaponTypes // TIPOS DE ARMA, INTRODUCIR AQUÍ EL NOMBRE DE LAS NUEVAS ARMAS
    {
        none, pistol, shotgun
    }
    public WeaponTypes currentWeapon; // Arma actual

    public float spread; // Dispersión para las armas con físicas

    public float fireRate; // Cadencia de disparo, cuanto menor el número, más rápido se puede disparar.
    public float cdShoot = 0f; // Cooldown de disparo;
    public bool cdBool; // Booleana cooldown de disparo;

    private void Awake()
    {
        controller = GetComponent<Controller>();
        currentWeaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // TRIGGER para disparar el arma
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

        // Inicio cooldown entre disparos
        if (!cdBool)
            cdShoot += Time.deltaTime;

        // Reset cooldown entre disparos
        if(cdShoot > fireRate)
        {
            cdShoot = 0f;
            cdBool = true;
        }
    }

    private void FixedUpdate()
    {
        // PRUEBAS CAMBIO DE SPRITE DEL ARMA, ELIMINAR CUANDO HAYA INTERACCIONES
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            currentWeapon = WeaponTypes.pistol;
            RenderSprite();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponTypes.shotgun;
            RenderSprite();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentWeapon = WeaponTypes.none;
            RenderSprite();
        }
    }

    /**
     * Ningún arma equipada
     */
    void None()
    {
        spread = 0;
        fireRate = 0;
    }

    /**
     * Pistola equipada
     */
    void Pistol()
    {
        this.spread = 2.5f; // Dispersión en la pistola para simular disparos de poca precisión
        fireRate = 0.5f;

        float randomBullet = Random.Range(this.spread * -1, this.spread);
        Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);
        Shoot(firePoint.position, firePoint.rotation * spread);
    }

    /**
     * Escopeta equipada
     */
    void Shotgun()
    {
        int pellets = 10;
        spread = 5f; // Dispersión reducida de 30 a 5 para que no sea tan facil acertar objetivos con este arma, (¿crear arma "Trabuco" de un solo disparo con dispersión 30?).
        fireRate = 1f;

        for (int i = 0; i < pellets; i++)
        {
            float randomBullet = Random.Range(this.spread * -1, this.spread);
            Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);

            Shoot(firePoint.position, firePoint.rotation * spread);
        }
    }

    /**
     * Disparo, requiere que se introduzca la posición 'position' y rotación 'rotation' del proyectil
     */
    void Shoot(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletPrefab, position, rotation);
    }

    /**
     * Método que controla el cambio de sprite del arma
     */
    void RenderSprite()
    {
        switch (currentWeapon)
        {
            case WeaponTypes.none:
                currentWeaponSprite.sprite = null;
                break;
            case WeaponTypes.pistol:
                currentWeaponSprite.sprite = spriteArray[0];
                break;
            case WeaponTypes.shotgun:
                currentWeaponSprite.sprite = spriteArray[1];
                break;
            default:
                break;
        }
    }
}
