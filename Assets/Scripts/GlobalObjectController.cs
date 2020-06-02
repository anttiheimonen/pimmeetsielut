using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectController : MonoBehaviour
{
    public static GlobalObjectController Instance;

    public int maxHealth;
    public int health;
    // public float XP;

    void Awake ()
    {
        Debug.Log("Awake kutsuttu");
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            health = maxHealth;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }

    public void SetHealth(int health) => this.health = health;

}