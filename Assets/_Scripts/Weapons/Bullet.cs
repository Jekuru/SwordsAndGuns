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
        rBody.AddForce(transform.right * speed, ForceMode2D.Impulse);
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
        LayerMask layers = LayerMask.GetMask("Player");

        RaycastHit2D hit = Physics2D.Raycast(origin: transform.position, direction: rBody.velocity, distance: rBody.velocity.magnitude * Time.deltaTime, layerMask: layers);
        
        if (hit.collider != null)
        {
            Debug.Log("Impact " + hit.collider.name);
            Debug.DrawRay(start: transform.position, dir: rBody.velocity, color: Color.green);

            hit.collider.GetComponent<PlayerStats>().healthPoints--;

            Destroy(gameObject);
        } else
        {
            Debug.Log("Not hit");
            Debug.DrawRay(transform.position, rBody.velocity, Color.red);
        }
    }
}
