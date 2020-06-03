using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public GameObject tentacle;
    public int numberOfTentacles;
    public Transform boss;
    public PolygonCollider2D pc2D;

    public int health;
    public AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TentacleAttack", 1, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeHit(int damage)
    {
        health = health - damage;
        Debug.Log("Boss saa damagea " + damage);
        Debug.Log("Bossin Life " + health);

        if(health <= 0)
            Die();
        else
            audioManager.Play("BossTakesHit");
    }


    private void Die()
    {
        audioManager.Play("BossDies");
        pc2D.enabled = false;
        CancelInvoke();
    }


    private void PlaySound(string sound)
    {
        audioManager.Play(sound);
    }


    private void TentacleAttack()
    {
        /*
        Spawn tentacles next to each other with tentacle-free safezone
        Some where in the area. Spawning starts from a random spot and
        next tentacle will be spawned right side of the previous one.
        If tentacle would be spawning too close to boss (closer than
        rightLimit), attacksize is reduced from tentacles x-position
        and it will spawn to left size of the attack area. Next tentacle
        will spawn again to right size of the this tentacle.
        After all tentacles have been spawned, there is safe zone for the
        player without tentacles.

        |              pla        BOSS
        | T T T T T T  yer T T T

        */
        int safezonesize = 3;
        int attacksize = safezonesize + numberOfTentacles;
        Vector3 bossPosition = boss.position;

        float rightLimit = bossPosition.x - 3;
        int attackStartPoint = Random.Range(numberOfTentacles, 4);

        for (int i = 0; i < numberOfTentacles; i++)
        {
            float locationX = bossPosition.x - attackStartPoint + i;
            if (locationX > rightLimit)
                locationX = locationX - attacksize;
            Vector3 location = new Vector3(locationX, 0, 0);

            Instantiate(tentacle, location, Quaternion.identity);
        }
    }


    private void DebugInfo()
    {
        Debug.Log(boss.position);
    }
}
