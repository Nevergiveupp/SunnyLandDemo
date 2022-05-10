using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;

    //private Collider2D coll;

    public Transform top, bottom;

    public float speed;

    // ���µ�ͷ��������
    private float topY, bottomY;

    // ��������
    private bool isUp = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = this.GetComponent<Rigidbody2D>();
        //coll = this.GetComponent<Collider2D>();
        // ��¼�����˶����µ�λ��Ȼ������
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

    // ��ӥ�ƶ�
    void Movement()
    {
        // ��������
        if (isUp)
        {
            // x���ٶȲ��䣬y����speed�ٶ��˶�
            rb.velocity = new Vector2(rb.velocity.x, speed);
            // �ɵ���ͷ��
            if (transform.position.y > topY)
            {
                // ���ϱ����Ϊfalse
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
