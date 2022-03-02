using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private InputController characterInput;
    private Controller characterController;
    private Animator animatiorController;
    public InputController deadInput; // Se establece desde el inspector
    public GameObject shieldSprite; // Se establece desde el inspector
    private float timeDead = 0;
    [SerializeField] private float despawnTime;

    private Move moveController;

    public int healthPoints = 1;
    public bool isDead;
    public bool isDesintegration;

    // Start is called before the first frame update
    void Start()
    {
        characterInput = GetComponent<Controller>().input;
        characterController = GetComponent<Controller>();
        moveController = GetComponent<Move>();
        animatiorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
        IsDead();
    }

    private void FixedUpdate()
    {
        if(healthPoints == 2)
        {
            shieldSprite.SetActive(true);
        } else
        {
            shieldSprite.SetActive(false);
        }
    }

    public void IsDead()
    {
        if (healthPoints <= 0)
        {
            isDead = true;
            characterController.input = deadInput;
            moveController.enabled = false;
            timeDead += Time.deltaTime;

            if (isDesintegration)
            {
                animatiorController.SetTrigger("Desintegration");
            }
            else
            {
                animatiorController.SetTrigger("Die");
            }
            

            if (timeDead >= despawnTime)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            isDead = false;
            characterController.input = characterInput;
            moveController.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Melee"))
        {
            healthPoints--;
            if (healthPoints <= 0)
            {
                gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                gameObject.GetComponent<Rigidbody2D>().AddTorque(-collision.GetComponentInParent<Rigidbody2D>().velocity.x * 8);
                gameObject.GetComponent<Rigidbody2D>().AddForce(collision.GetComponentInParent<Rigidbody2D>().velocity * 3, ForceMode2D.Impulse);

                //gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                gameObject.tag = "Death";
            }
        }
    }
}
