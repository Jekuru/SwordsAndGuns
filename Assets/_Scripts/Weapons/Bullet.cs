using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rBody;
    public float travelTime;

    // Start is called before the first frame update
    void Start()
    {
        //rBody.velocity = transform.right * speed;
        rBody.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
        travelTime += Time.deltaTime;
        if(travelTime > 5f)
        {
            Destroy(gameObject);
        }

        
    }

    private void FixedUpdate()
    {
        Detection();
    }


    /**
     * Nota:
     * Reducida la velocidad de las balas a 40 porque atraviesan los objetos.
     * Revisar colisión de la bala con el jugador, ya que no parece colisionar 2DCollider, igual es percepcion propia debido al trail y sprite oculto de la bala.
     * 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name != "FirePoint" && collision.name != "ProtoBullet(Clone)")
        {
            //Debug.Log(collision.name);
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerStats>().healthPoints--;
            }
            Destroy(gameObject);
        }
    }     */

    private void Detection()
    {
        LayerMask layers = LayerMask.GetMask("Player");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rBody.velocity, rBody.velocity.magnitude * Time.deltaTime, layers);
        
        if (hit.collider != null)
        {
            Debug.Log("Impact " + hit.collider.name);
            Debug.DrawRay(transform.position, rBody.velocity, Color.green);
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerStats>().healthPoints--;
            }
            Destroy(gameObject);
        } else
        {
            Debug.Log("Not hit");
            Debug.DrawRay(transform.position, rBody.velocity, Color.red);
        }
    }
}
