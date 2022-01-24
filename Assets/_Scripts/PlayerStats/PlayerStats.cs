using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthPoints = 1;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsDead();
    }

    public void IsDead()
    {
        if(healthPoints <= 0)
        {
            isDead = true;
        } else
        {
            isDead = false;
        }
    }
}
