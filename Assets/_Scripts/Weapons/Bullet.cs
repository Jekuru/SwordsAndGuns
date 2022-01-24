using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().healthPoints--;
        }
        Destroy(gameObject);
    }
}
