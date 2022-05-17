using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Opossum : Enemy
{
    private Rigidbody2D rb;

    //private Collider2D coll;

    public Transform left, right;

    public float speed;

    // ���ҵ�ͷ�������
    private float leftX, rightX;

    // ��������
    private bool isLeft = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = this.GetComponent<Rigidbody2D>();
        // �������ҵ�����˼�ĸ��ӹ�ϵ
        transform.DetachChildren();
        // ��¼�����˶����ҵ�λ��Ȼ������
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

    // �ƶ�
    void Movement()
    {
        
        // ��������
        if (isLeft)
        {
            // y���ٶȲ��䣬x����speed�ٶ��˶�
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            // �����ߵ���ͷ��
            if (transform.position.x < leftX)
            {
                // ��ͷ
                this.transform.localScale = new Vector3(-1, 1, 1);
                // ��������Ϊfalse
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
