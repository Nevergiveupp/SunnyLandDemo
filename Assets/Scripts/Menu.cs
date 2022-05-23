using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;

    public AudioMixer audioMixer;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UIEnable()
    {
        // ����UI
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
        Debug.Log("UI������");
    }

    // ��ͣ��Ϸ
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        // ʱ��ֹͣ
        Time.timeScale = 0f;
    }

    // �ص���Ϸ
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        // ʱ��ָ�
        Time.timeScale = 1f;
    }

    public void SetVolume(float value)
    {
        // AudioMixer�е�Exposed Parameter����Ϊvalue
        audioMixer.SetFloat("MainVolume", value);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // ��������
        StaticCount.cherryCount = 0;
        StaticCount.gemCount = 0;
        SceneManager.LoadScene(0);

    }

}
