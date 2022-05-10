using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;

    //private Animator anim;

    private Collider2D coll;

    [Tooltip("Layer")]
    public LayerMask ground;

    [Tooltip("Enemy Leftpoint Position")]
    public Transform leftpoint;

    [Tooltip("Enemy Rightpoint Position")]
    public Transform rightpoint;

    // 弹跳高度
    public float jumpForce;

    // 横向移速
    public float speed;

    // 左右侧调头点横坐标
    private float leftx, rightx;

    // 敌人朝向
    private bool faceleft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        // 调用父类的Start()方法
        base.Start();
        rb = this.GetComponent<Rigidbody2D>();
        // anim = this.GetComponent<Animator>();
        coll = this.GetComponent<Collider2D>();

        // 消除左右点与敌人间的父子关系
        transform.DetachChildren();
        // 记录敌人运动左右点位，然后销毁
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // 用animation event调用
        // Movement();
        SwitchAnim();
    }

    // 青蛙移动
    void Movement()
    {
        // 如果面向左
        if (faceleft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                // 向左跳
                rb.velocity = new Vector2(-speed, jumpForce);
            }

            // 向左边走超过左侧调头点
            if (this.transform.position.x < leftx)
            {
                // 调头
                this.transform.localScale = new Vector3(-1, 1, 1);
                faceleft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                // 向右跳
                rb.velocity = new Vector2(speed, jumpForce);
            }

            // 向右边走超过右侧调头点
            if (this.transform.position.x > rightx)
            {
                // 调头
                this.transform.localScale = new Vector3(1, 1, 1);
                faceleft = true;
            }
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);

            }
        }
        // 如果接触地面，且正在下落
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            // 不在下落，切换回闲置状态
            anim.SetBool("falling", false);
        }
    }

    
}
