using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]

public class Weapon : MonoBehaviour
{ 
    public GameObject weapon; // GameObject del arma
    public Transform firePoint; // Desde donde sale el proyectil
    public Sprite[] spriteArray; // Array de sprites, añadir desde el inspector
    private Controller controller; // Inputs del jugador
    public GameObject bulletPrefab; // Proyectil para las armas a distancia que utlizan físicas
    public SpriteRenderer currentWeaponSprite; // Sprite actual del arma equipada
    public GameObject throwedWeapon; // Arma duplicada que se lanzará

    public int ammo; // Munición actual del arma.
    public int maxAmmo; // Munición máxima del arma.
    public float spread; // Dispersión para las armas con físicas
    public float fireRate; // Cadencia de disparo, cuanto menor el número, más rápido se puede disparar.
    public float cdShoot = 0f; // Cooldown de disparo;
    public bool cdBool; // Booleana cooldown de disparo;
    public bool meleeAttack; // Booleana para triggear animacion de ataque en el anim;

    public GameObject clonedWeapon;
    public bool destroyThrowedWeapon; // Booleana que activa el tiempo hasta que se destruya el arma
    public float throwTimer; // Tiempo hasta que se destruya el arma arrojada
    public float throwTimerMax = 10f; // Tiempo establecido para que se destruya el arma arrojada

    public enum WeaponTypes // TIPOS DE ARMA, INTRODUCIR AQUÍ EL NOMBRE DE LAS NUEVAS ARMAS
    {
        none,                                       // Nada
        sword,                                      // Melee
        pistol, shotgun, sniper,                    // Distancia físicas
        raygun,                                     // Distancia raycast
                                                    // Proyectil AoE
    }
    public WeaponTypes currentWeapon; // Arma actual

    // TODO: Falta por PROBAR MUNICIÓN.
    // TODO: Solventar problema al tirar el arma.

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
                // NINGUNA
                case WeaponTypes.none:
                    None();
                    break;
                // CUERPO A CUERPO
                case WeaponTypes.sword:
                    Melee();
                    break;
                // PROYECTIL CON FÍSICA
                case WeaponTypes.pistol:
                    Pistol();
                    break;
                case WeaponTypes.shotgun:
                    Shotgun();
                    break;
                case WeaponTypes.sniper:
                    Sniper();
                    break;
                // PROYECTIL RAYCAST
                case WeaponTypes.raygun:
                    Raygun();
                    break;
                // PROYECTIL AoE
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

        if (destroyThrowedWeapon)
        {
            throwTimer += Time.deltaTime;
            if(throwTimer >= throwTimerMax)
            {
                Destroy(clonedWeapon);
                destroyThrowedWeapon = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // TODO: PRUEBAS CAMBIO DE ARMA Y SPRITE, ELIMINAR CUANDO HAYA INTERACCIONES CON EL SPAWNER
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            currentWeapon = WeaponTypes.sword;
            RenderSprite();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponTypes.pistol;
            RenderSprite();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = WeaponTypes.shotgun;
            RenderSprite();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeapon = WeaponTypes.sniper;
            RenderSprite();
        }
        /*if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentWeapon = WeaponTypes.raygun;
            RenderSprite();
        }*/
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentWeapon = WeaponTypes.none;
            RenderSprite();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ThrowGun();
        }
    }

    #region Comportamiento armas
    /**
     * Ningún arma equipada
     */
    void None()
    {
        spread = 0;
        fireRate = 0;
    }

    /**
     * Arma melee equipada
     */
    void Melee()
    {
        fireRate = 0.5f;
        spread = 0;

        meleeAttack = true;
    }

    /**
     * Pistola equipada
     */
    void Pistol()
    {
        maxAmmo = 7;
        ammo++;
        this.spread = 2.5f; // Dispersión en la pistola para simular disparos de poca precisión
        fireRate = 0.5f;

        float randomBullet = Random.Range(this.spread * -1, this.spread);
        Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);
        Shoot(firePoint.position, firePoint.rotation * spread);
        if(ammo >= maxAmmo){
            ThrowGun();
        }
    }

    /**
     * Escopeta equipada
     */
    void Shotgun()
    {
        maxAmmo = 2;
        ammo++;
        int pellets = 10; // Número de proyectiles que se instanciarán
        spread = 5f; // Dispersión reducida de 30 a 5 para que no sea tan facil acertar objetivos con este arma, (¿crear arma "Trabuco" de un solo disparo con dispersión 30?).
        fireRate = 1f;

        for (int i = 0; i < pellets; i++)
        {
            float randomBullet = Random.Range(this.spread * -1, this.spread);
            Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);

            Shoot(firePoint.position, firePoint.rotation * spread);
        }
        if (ammo >= maxAmmo)
        {
            ThrowGun();
        }
    }

     /**
     * Sniper equipado
     */
    void Sniper()
    {
        maxAmmo = 5;
        ammo++;
        this.spread = 0.1f;
        fireRate = 1.5f;

        float randomBullet = Random.Range(this.spread * -1, this.spread);
        Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);

        Shoot(firePoint.position, firePoint.rotation * spread);
        if (ammo >= maxAmmo)
        {
            ThrowGun();
        }
    }

    /**
     * Raygun/arma láser equipada
     */

    void Raygun()
    {
        maxAmmo = 1;
        ammo++;
        spread = 0;
        fireRate = 2f;

        // TODO: Crear o instanciar el rayo.

        if (ammo >= maxAmmo)
        {
            ThrowGun();
        }
    }

    #endregion

    /**
     * Método que controla el cambio de sprite del arma
     */
    void RenderSprite()
    {
        switch (currentWeapon)
        {
            // NINGUNA
            case WeaponTypes.none:
                currentWeaponSprite.sprite = null;
                break;
            // CUERPO A CUERPO
            case WeaponTypes.sword:
                currentWeaponSprite.sprite = spriteArray[1];
                break;
            // PROYECTIL CON FÍSICA
            case WeaponTypes.pistol:
                currentWeaponSprite.sprite = spriteArray[0];
                break;
            case WeaponTypes.shotgun:
                currentWeaponSprite.sprite = spriteArray[0];
                break;
            case WeaponTypes.sniper:
                currentWeaponSprite.sprite = spriteArray[0];
                break;
            // PROYECTIL RAYCAST
            case WeaponTypes.raygun:
                currentWeaponSprite.sprite = null;
                break;
            // PROYECTIL AoE
            default:
                break;
        }
    }

    /**
     * Disparo, requiere que se introduzca la posición 'position' y rotación 'rotation' del proyectil
     */
    void Shoot(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletPrefab, position, rotation);
    }

    /*
     * Tirar arma
     */
    void ThrowGun()
    {
        if (currentWeapon == WeaponTypes.none)
            return;

        throwedWeapon.GetComponent<SpriteRenderer>().sprite = currentWeaponSprite.sprite;
        clonedWeapon = Instantiate(throwedWeapon, weapon.transform.position, weapon.transform.rotation);
        clonedWeapon.GetComponent<Rigidbody2D>().AddForce(clonedWeapon.transform.right * 1.8f, ForceMode2D.Impulse);
        currentWeapon = WeaponTypes.none;
        RenderSprite();

        destroyThrowedWeapon = true;
    }
}
