using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Opossum : Enemy
{
    private Rigidbody2D rb;

    //private Collider2D coll;

    public Transform left, right;

    public float speed;

    // 左右调头点横坐标
    private float leftX, rightX;

    // 正在向左
    private bool isLeft = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = this.GetComponent<Rigidbody2D>();
        // 消除左右点与敌人间的父子关系
        transform.DetachChildren();
        // 记录敌人运动左右点位，然后销毁
        leftX = left.position.x;
        rightX = right.position.x;
        //Destroy(left.gameObject);
        //Destroy(right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    // 移动
    void Movement()
    {
        
        // 正在向左
        if (isLeft)
        {
            // y轴速度不变，x轴以speed速度运动
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            // 向左走到调头点
            if (transform.position.x < leftX)
            {
                // 调头
                this.transform.localScale = new Vector3(-1, 1, 1);
                // 向左标记置为false
                isLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightX)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
                isLeft = true;
            }
        }
    }
}
