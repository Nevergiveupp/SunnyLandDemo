using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//����������
public class SceneMgr : MonoBehaviour
{
    //����
    public static SceneMgr ins;
    private void Awake()
    {
        if (ins == null) { ins = this; DontDestroyOnLoad(this); }
        else if (ins != this) { Destroy(gameObject); }
    }

    //��������
    Dictionary<string, int> sceneOneshotData = null;

    //��������
    private void WriteSceneData(Dictionary<string, int> data)
    {
        if (sceneOneshotData != null)
        {
            Debug.LogError("�л����ݲ�Ϊ�գ���һ���л�����������û�б���ȡ");
        }
        sceneOneshotData = data;
    }

    //ȡ������
    public Dictionary<string, int> ReadSceneData()
    {
        Dictionary<string, int> tempData = sceneOneshotData;
        sceneOneshotData = null;//���
        return tempData;
    }

    //ǰ���³���
    public void ToNewScene(string sceneName, Dictionary<string, int> param = null)
    {
        //д������
        this.WriteSceneData(param);
        //������һ������
        SceneManager.LoadScene(sceneName);
    }
}


