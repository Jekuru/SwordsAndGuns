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
    /**
     * Nota:
     * Reducida la velocidad de las balas a 40 porque atraviesan los objetos.
     * Revisar colisión de la bala con el jugador, ya que no parece colisionar 2DCollider, igual es percepcion propia debido al trail y sprite oculto de la bala.
     * 
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name != "FirePoint" && collision.name != "ProtoBullet(Clone)")
        {
            Debug.Log(collision.name);
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerStats>().healthPoints--;
            }
            Destroy(gameObject);
        }
    }
}
