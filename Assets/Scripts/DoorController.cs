using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{

    public int LevelToLoad;

    void OnTriggerEnter2D (Collider2D col)
    {
        if(col.CompareTag("Player")) {
            SceneManager.LoadScene(LevelToLoad);
        }

    }

    void OnTriggerStay2D (Collider2D col)
    {

    }

    void OnTriggerExit2D (Collider2D col)
    {
        Debug.Log("Exit");
    }
}
