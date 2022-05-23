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
        // 启用UI
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
        Debug.Log("UI已启用");
    }

    // 暂停游戏
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        // 时间停止
        Time.timeScale = 0f;
    }

    // 回到游戏
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        // 时间恢复
        Time.timeScale = 1f;
    }

    public void SetVolume(float value)
    {
        // AudioMixer中的Exposed Parameter设置为value
        audioMixer.SetFloat("MainVolume", value);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // 积分清零
        StaticCount.cherryCount = 0;
        StaticCount.gemCount = 0;
        SceneManager.LoadScene(0);

    }

}
