using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 2;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void takeHit()
    {
        currentHealth--;

        if(currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Debug.Log("Kuoli saatana");
        transform.Rotate(0, 0, -90);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
