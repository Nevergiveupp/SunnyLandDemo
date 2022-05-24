using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : MonoBehaviour
{
    public PlayerController pc;
    

    // Update is called once per frame
    void Update()
    {
        // °´ÏÂE
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
