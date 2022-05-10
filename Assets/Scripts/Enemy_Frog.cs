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

    // �����߶�
    public float jumpForce;

    // ��������
    public float speed;

    // ���Ҳ��ͷ�������
    private float leftx, rightx;

    // ���˳���
    private bool faceleft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        // ���ø����Start()����
        base.Start();
        rb = this.GetComponent<Rigidbody2D>();
        // anim = this.GetComponent<Animator>();
        coll = this.GetComponent<Collider2D>();

        // �������ҵ�����˼�ĸ��ӹ�ϵ
        transform.DetachChildren();
        // ��¼�����˶����ҵ�λ��Ȼ������
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // ��animation event����
        // Movement();
        SwitchAnim();
    }

    // �����ƶ�
    void Movement()
    {
        // ���������
        if (faceleft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                // ������
                rb.velocity = new Vector2(-speed, jumpForce);
            }

            // ������߳�������ͷ��
            if (this.transform.position.x < leftx)
            {
                // ��ͷ
                this.transform.localScale = new Vector3(-1, 1, 1);
                faceleft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                // ������
                rb.velocity = new Vector2(speed, jumpForce);
            }

            // ���ұ��߳����Ҳ��ͷ��
            if (this.transform.position.x > rightx)
            {
                // ��ͷ
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
        // ����Ӵ����棬����������
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            // �������䣬�л�������״̬
            anim.SetBool("falling", false);
        }
    }

    
}
