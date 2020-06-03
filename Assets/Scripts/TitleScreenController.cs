using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalObjectController.Instance.DestroyInstance();

    }

    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("A key or mouse click has been detected");
            SceneManager.LoadScene(1);
        }
    }
}
