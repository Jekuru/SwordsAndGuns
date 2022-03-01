using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private Rigidbody2D rBody;
    [SerializeField] private float travelTime;
    public PhysicsMaterial2D stickyMat;

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
        Detection();
    }

    private void Detection()
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
            hit.collider.GetComponent<PlayerStats>().healthPoints--;
            if(hit.collider.GetComponent<PlayerStats>().healthPoints <= 0)
            {
                hit.collider.GetComponent<Rigidbody2D>().AddForce(rBody.velocity * 0.4f, ForceMode2D.Impulse);
                hit.collider.GetComponent<Rigidbody2D>().AddTorque(-rBody.velocity.x * 8);
                hit.collider.GetComponent<Rigidbody2D>().freezeRotation = false;

                //hit.collider.GetComponent<CapsuleCollider2D>().enabled = false;
                hit.collider.tag = "Death";
            }

            Destroy(gameObject);
        }
    }
}
