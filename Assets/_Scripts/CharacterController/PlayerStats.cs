using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
            return;

        characterInput = GetComponent<Controller>().input;
        characterController = GetComponent<Controller>();
        moveController = GetComponent<Move>();
        animatiorController = GetComponent<Animator>();
        photonView.RPC("ShareName", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        IsDead();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        if (healthPoints == 2)
        {
            shieldSprite.SetActive(true);
            photonView.RPC("ShieldOn", RpcTarget.Others);
        } else
        {
            shieldSprite.SetActive(false);
            photonView.RPC("ShieldOff", RpcTarget.Others);
        }
    }

    public void IsDead()
    {
        if (healthPoints <= 0)
        {
            photonView.RPC("Die", RpcTarget.Others, healthPoints);
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
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            isDead = false;
            characterController.input = characterInput;
            moveController.enabled = true;
        }
    }

    [PunRPC]
    private void Die(int hp)
    {
        healthPoints = hp;
        isDead = true;
    }
    
    [PunRPC]
    private void ShieldOn()
    {
        shieldSprite.SetActive(true);
    }

    [PunRPC]
    private void ShieldOff()
    {
        shieldSprite.SetActive(false);
    }

    [PunRPC]
    private void ShareName(string userId)
    {
        gameObject.name = userId;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;
        if (collision.transform.CompareTag("Melee"))
        {
            healthPoints--;
            if (healthPoints <= 0)
            {
                gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                gameObject.GetComponent<Rigidbody2D>().AddTorque(-collision.GetComponentInParent<Rigidbody2D>().velocity.x * 8);
                gameObject.GetComponent<Rigidbody2D>().AddForce(collision.GetComponentInParent<Rigidbody2D>().velocity * 3, ForceMode2D.Impulse);
                gameObject.tag = "Death";
            }
        }
    }
}
