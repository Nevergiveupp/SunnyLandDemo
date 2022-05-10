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

    // 销毁敌人
    public void Death()
    {
        // 将敌人碰撞体禁用，防止二次碰撞
        this.GetComponent<Collider2D>().enabled = false;
        // 摧毁敌人
        Destroy(this.gameObject);

    }

    // 供PlayerController调用，销毁敌人
    public void JumpOn()
    {
        // 播放死亡动画
        anim.SetTrigger("death");
        // 播放音效
        deathAudio.Play();
    }

}
