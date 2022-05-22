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
            Dictionary<string, int> param = new Dictionary<string, int>();
            param.Add("cherryCount", pc.cherryCount);
            param.Add("gemCount", pc.gemCount);
            param.Add("currentHealth", pc.currentHealth);
            SceneMgr.ins.ToNewScene("Level02", param);
            foreach (KeyValuePair<string, int> kvp in param)
            {
                Debug.Log("l01key:" + kvp.Key + "l01value:" + kvp.Value);
            }
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
