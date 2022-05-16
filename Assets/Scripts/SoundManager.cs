using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Ύ²Μ¬ΚµΜε
    public static SoundManager instance;

    public AudioSource audioSource;

    [SerializeField]
    public AudioClip jumpAudio, hurtAudio, getItemAudio;

    private void Awake()
    {
        instance = this;
    }

    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void GetItemAudio()
    {
        audioSource.clip = getItemAudio;
        audioSource.Play();
    }
}
