using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]

public class Weapon : MonoBehaviour
{ 
    // Controlador del jugador
    private Controller controller; // Inputs del jugador

    // Variables a introducir manualmente desde el inspector
    [SerializeField] public GameObject weapon; // GameObject del arma TODO: Cambiado a public
    [SerializeField] private Transform firePoint; // Desde donde sale el proyectil
    [SerializeField] private Sprite[] spriteArray; // Array de sprites, a�adir desde el inspector
    [SerializeField] private GameObject bulletPrefab; // Proyectil para las armas a distancia que utlizan f�sicas
    [SerializeField] private GameObject laserBeamPrefab; // Proyectil con Line Renderer para la raygun
    [SerializeField] private GameObject throwedWeapon; // Arma duplicada que se lanzar�

    // Sprite del arma catual
    private SpriteRenderer currentWeaponSprite; // Sprite actual del arma equipada

    // Munici�n
    [SerializeField] private int ammo; // Munici�n actual del arma.
    [SerializeField] private int maxAmmo; // Munici�n m�xima del arma.

    // Cadencia y dispersi�n
    private float fireRate; // Cadencia de disparo, cuanto menor el n�mero, m�s r�pido se puede disparar.
    private float spread; // Dispersi�n para las armas con f�sicas
    
    // Cooldowns
    private bool cdBool; // Booleana cooldown de disparo;
    private float cdShoot = 0f; // Cooldown de disparo;
    
    // Arma clonada
    private GameObject clonedWeapon;

    // A partir de aqu�, variables p�blicas. �No cambiar a private!
    public bool meleeAttack; // Booleana para triggear animacion de ataque en el anim;

    // Armas
    public enum WeaponTypes // TIPOS DE ARMA, INTRODUCIR AQU� EL NOMBRE DE LAS NUEVAS ARMAS
    {
        none,                                       // Nada
        sword,                                      // Melee
        pistol, shotgun, sniper,                    // Distancia f�sicas
        raygun,                                     // Distancia raycast
                                                    // Proyectil AoE
    }
    // Arma equipada actualmente
    public WeaponTypes currentWeapon; // Arma actual

    private void Awake()
    {
        controller = GetComponent<Controller>();
        currentWeaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // TRIGGER para disparar el arma
        WeaponTrigger();

        // COOLDOWN entre disparos, frecuencia de disparo
        WeaponCooldown();
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
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentWeapon = WeaponTypes.raygun;
            RenderSprite();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentWeapon = WeaponTypes.none;
            RenderSprite();
        }
        if (controller.input.RetrieveThrowInput())
        {
            ThrowGun();
        }
    }

    /**
    * Ejecuta el comportamiento de un arma u otra al pulsar el bot�n de disparo
    */
    void WeaponTrigger()
    {
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
                // PROYECTIL CON F�SICA
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
    }

    /**
    * Disparo, requiere que se introduzca la posici�n 'position' y rotaci�n 'rotation' del proyectil
    */
    void Shoot(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletPrefab, position, rotation);
    }

    /**
     * Controla la frecuencia con la que se puede disparar el arma
     */
    void WeaponCooldown()
    {
        // Inicio cooldown entre disparos
        if (!cdBool)
            cdShoot += Time.deltaTime;

        // Reset cooldown entre disparos
        if (cdShoot > fireRate)
        {
            cdShoot = 0f;
            cdBool = true;
        }
    }

    /**
     * M�todo que controla el cambio de sprite del arma
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
                currentWeaponSprite.sprite = spriteArray[0];
                break;
            // PROYECTIL CON F�SICA
            case WeaponTypes.pistol:
                currentWeaponSprite.sprite = spriteArray[1];
                break;
            case WeaponTypes.shotgun:
                currentWeaponSprite.sprite = spriteArray[2];
                break;
            case WeaponTypes.sniper:
                currentWeaponSprite.sprite = spriteArray[3];
                break;
            // PROYECTIL RAYCAST
            case WeaponTypes.raygun:
                currentWeaponSprite.sprite = spriteArray[4];
                break;
            // PROYECTIL AoE
            default:
                break;
        }
    }

    /*
     * Tirar arma
     */
    void ThrowGun()
    {
        ammo = 0;
        if (currentWeapon == WeaponTypes.none)
            return;

        throwedWeapon.GetComponent<SpriteRenderer>().sprite = currentWeaponSprite.sprite;
        clonedWeapon = Instantiate(throwedWeapon, weapon.transform.position, weapon.transform.rotation);
        clonedWeapon.GetComponent<Rigidbody2D>().AddForce(clonedWeapon.transform.right * 1.8f, ForceMode2D.Impulse);
        clonedWeapon.GetComponent<Rigidbody2D>().AddTorque(0.5f);
        currentWeapon = WeaponTypes.none;
        RenderSprite();
    }

    #region Comportamiento armas
    /**
     * Ning�n arma equipada
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
        this.spread = 2.5f; // Dispersi�n en la pistola para simular disparos de poca precisi�n
        fireRate = 0.5f;

        float randomBullet = Random.Range(this.spread * -1, this.spread);
        Quaternion spread = Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, randomBullet);
        Shoot(firePoint.position, firePoint.rotation * spread);

        if (ammo >= maxAmmo)
        {
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
        int pellets = 10; // N�mero de proyectiles que se instanciar�n
        spread = 5f; // Dispersi�n reducida de 30 a 5 para que no sea tan facil acertar objetivos con este arma, (�crear arma "Trabuco" de un solo disparo con dispersi�n 30?).
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
     * Raygun/arma l�ser equipada
     */
    void Raygun()
    {
        maxAmmo = 1;
        ammo++;
        spread = 0;
        fireRate = 2f;

        Instantiate(laserBeamPrefab, firePoint.position, firePoint.rotation);

        if (ammo >= maxAmmo)
        {
            ThrowGun();
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin: firePoint.transform.position, direction: firePoint.transform.right, distance: 100F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Raygun hit ground");
                return;
            }

            if (hit.collider.CompareTag("Player"))
            {
                PlayerStats playerStats = hit.transform.GetComponent<PlayerStats>();
                playerStats.healthPoints--;
                if (hit.transform.GetComponent<PlayerStats>())
                {
                    hit.transform.GetComponent<PlayerStats>().isDesintegration = true;
                }
                Debug.Log(hit.collider.gameObject.name + " impactado por raygun.");
            }
        }
    }

    #endregion

}
