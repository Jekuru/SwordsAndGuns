using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowedWeaponCleaner : MonoBehaviour
{
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.right * 1.8f, ForceMode2D.Impulse);
        gameObject.GetComponent<Rigidbody2D>().AddTorque(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }
}
