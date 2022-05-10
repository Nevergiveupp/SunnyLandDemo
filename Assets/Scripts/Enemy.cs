using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;

    protected AudioSource deathAudio;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = this.GetComponent<Animator>();
        deathAudio = this.GetComponent<AudioSource>();
    }

    // ���ٵ���
    public void Death()
    {
        // ��������ײ����ã���ֹ������ײ
        this.GetComponent<Collider2D>().enabled = false;
        // �ݻٵ���
        Destroy(this.gameObject);

    }

    // ��PlayerController���ã����ٵ���
    public void JumpOn()
    {
        // ������������
        anim.SetTrigger("death");
        // ������Ч
        deathAudio.Play();
    }

}
