using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectController : MonoBehaviour
{
    public static GlobalObjectController Instance;

    public int maxHealth;
    public int health;
    public int attackDamage;

    void Awake ()
    {
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
    public void SetAttackDamage(int damage) => this.attackDamage = damage;

    public void DestroyInstance() => Destroy (gameObject);

}