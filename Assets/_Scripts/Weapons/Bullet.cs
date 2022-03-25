using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private Rigidbody2D rBody;
    [SerializeField] private float travelTime;
    [SerializeField] private GameObject targetHit;
    public PhysicsMaterial2D stickyMat;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rBody.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
    }

    void Update()
    {
        travelTime += Time.deltaTime;
        if (travelTime > 5f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        BulletDetection();
    }

    private void BulletDetection()
    {
        LayerMask layers = LayerMask.GetMask("Player", "Ground");

        RaycastHit2D hit = Physics2D.Raycast(origin: transform.position, direction: rBody.velocity, distance: rBody.velocity.magnitude * Time.deltaTime, layerMask: layers);
        
        if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            return;
        }
        else if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            targetHit = hit.collider.gameObject;

            photonView.RPC("ShareTargetHit", RpcTarget.All);
            //hit.collider.GetComponent<PlayerStats>().healthPoints--;


            Destroy(gameObject);
        }
    }

    [PunRPC]
    void ShareTargetHit()
    {
        targetHit.GetComponent<PlayerStats>().healthPoints--;
        if (targetHit.GetComponent<PlayerStats>().healthPoints <= 0)
        {
            targetHit.GetComponent<Rigidbody2D>().AddForce(rBody.velocity * 0.4f, ForceMode2D.Impulse);
            targetHit.GetComponent<Rigidbody2D>().AddTorque(-rBody.velocity.x * 8);
            targetHit.GetComponent<Rigidbody2D>().freezeRotation = false;

            targetHit.tag = "Death";
        }
    }
}
