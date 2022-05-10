using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;

    //private Collider2D coll;

    public Transform top, bottom;

    public float speed;

    // 上下调头点纵坐标
    private float topY, bottomY;

    // 正在上升
    private bool isUp = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = this.GetComponent<Rigidbody2D>();
        //coll = this.GetComponent<Collider2D>();
        // 记录敌人运动上下点位，然后销毁
        topY = top.position.y;
        bottomY = bottom.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    // 老鹰移动
    void Movement()
    {
        // 正在向上
        if (isUp)
        {
            // x轴速度不变，y轴以speed速度运动
            rb.velocity = new Vector2(rb.velocity.x, speed);
            // 飞到调头点
            if (transform.position.y > topY)
            {
                // 向上标记置为false
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < bottomY)
            {
                isUp = true;
            }
        }
    }
}
