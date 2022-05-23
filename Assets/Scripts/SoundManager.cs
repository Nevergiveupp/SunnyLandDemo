using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Ύ²Μ¬ΚµΜε
    public static SoundManager instance;

    public AudioSource audioSource;

    [SerializeField]
    public AudioClip jumpAudio, hurtAudio, getItemAudio, fireAudio, slashAudio, enrageAudio, swordAudio;

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

    public void FireAudio()
    {
        audioSource.clip = fireAudio;
        audioSource.Play();
    }

    public void SlashAudio()
    {
        audioSource.clip = slashAudio;
        audioSource.Play();
    }

    public void EnrageAudio()
    {
        audioSource.clip = enrageAudio;
        audioSource.Play();
    }

    public void SwordAudio()
    {
        audioSource.clip = swordAudio;
        audioSource.Play();
    }
}
