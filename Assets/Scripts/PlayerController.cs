using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Player Rigidbody")]
    //[SerializeField] // ʹprivate�����ڼ������չʾ��debug��
    private Rigidbody2D rb;

    [Tooltip("Player Animator")]
    private Animator anim;

    // �°�����ײ��
    [Tooltip("Player Collider")]
    public Collider2D coll;

    // �ϰ�����ײ�壨�¶�ʱ�رգ�
    public Collider2D disColl;

    // ͷ������
    public Transform cellingCheck;

    [Tooltip("Jump Audio")]
    public AudioSource jumpAudio;

    [Tooltip("Hurt Audio")]
    public AudioSource hurtAudio;

    [Tooltip("Get Item Audio")]
    public AudioSource getItemAudio;

    [Tooltip("Player Move Speed")]
    public float speed;

    [Tooltip("Player Jump Force")]
    public float jumpForce;

    [Tooltip("Layer")]
    public LayerMask ground;

    [SerializeField]
    private int cherryCount;

    [SerializeField]
    private TMP_Text cherryNum;

    // �����Ƿ����ˣ�Ĭ��false
    private bool isHurt;


    // Start is called before the first frame update
    void Start()
    {
        // �ȶ�֡��Ϊ60
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // ���û����
        if (!isHurt)
        {
            // ִ�н�ɫ����
            Movement();
        }
        // ������ˣ�ֱ��ִ���л�����
        SwitchAnim();
    }

    // Update is called once per frame
    private void Update()
    {
        // ��ɫ��Ծ
        Jump();
        // ��ɫ�¶�
        Crouch();
        cherryNum.text = cherryCount.ToString();
    }

    // ��ɫ�ƶ�
    void Movement()
    {
        // ��ȡˮƽ���ֵ���洢�ڱ�����
        float horizontalValue = Input.GetAxis("Horizontal");
        // ��ɫ�泯����
        float faceDirection = Input.GetAxisRaw("Horizontal");

        // ��ɫ�ƶ�
        if (horizontalValue != 0)
        {
            // ���ý�ɫ�ٶ�
            rb.velocity = new Vector2(horizontalValue * speed * Time.fixedDeltaTime, rb.velocity.y); // ע�⣺FixUpdate()������ʹ��Time.fixedDeltaTime����ʹ���������
            // ���ö�������
            anim.SetFloat("running", Mathf.Abs(faceDirection));
        }

        // ��ɫ����
        if (faceDirection != 0)
        {
            this.transform.localScale = new Vector3(faceDirection, 1, 1);
        }

    }

    // �л�����
    void SwitchAnim()
    {
        // ��ʼ��idle
        //anim.SetBool("idle", false);

        // ����Ӹߴ�����ʱ�ȵ��˻����˵�����
        // �������û���ٶ��Ҳ��Ӵ�����
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            // ��ɫ������
            anim.SetBool("falling", true);
        }

        // �������Ծ
        if (anim.GetBool("jumping"))
        {
            // ���������
            if(rb.velocity.y < 0)
            {
                // ���ö���Ϊ����
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        // �����ɫ����
        else if (isHurt)
        {
            // ����������Ϊtrue
            anim.SetBool("hurt", true);
            // ����runningֵ��Ϊ0��������˵�����ԭ���ܲ�������
            anim.SetFloat("running", 0);

            // �жϽ�ɫ�ٶ�С��0.1������״̬����
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                // ���������꣬����״̬��Ϊfalse������״̬��Ϊtrue
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                // ����Ϊδ����
                isHurt = false;
            }
        }

        // �����ɫ�Ӵ�����
        else if (coll.IsTouchingLayers(ground))
        {
            // ���ö���Ϊ����
            anim.SetBool("falling", false);
            //anim.SetBool("idle", true);
        }
    }

    // ��ײ������
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // �ռ���Ʒ
        // �����ײ��Collections��������
        if (collider.tag == "Collections")
        {
            // �����Ч�������ε�����
            collider.tag = "null";
            // ������Ч
            getItemAudio.Play();
            // ���ٵ�ǰ����ײ����
            //Destroy(collider.gameObject);
            // ������һ
            //cherryCount += 1;
            // ������ײ�嶯��
            collider.GetComponent<Animator>().Play("IsGot");
            //cherryNum.text = cherryCount.ToString();
        }

        // ��ɫ����
        if (collider.tag == "DeadLine")
        {
            // ����������Դ
            this.GetComponent<AudioSource>().enabled = false;
            // �ӳ�ִ�����õ�ǰ����
            Invoke("Restart", 2f);

        }
    }

    // �������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �����ײ������
        if (collision.gameObject.tag == "Enemies")
        {
            // ��ȡ���˶���
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // �������׹��
            if (anim.GetBool("falling"))
            {
                // ���ŵ�����������
                enemy.JumpOn();

                // ��ɫԾ��
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("jumping", true);
            } 
            // ��������ڵ�����࣬���˵������
            else if (this.transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-4, rb.velocity.y);
                // ������Ч
                hurtAudio.Play();
                // ���˱��
                isHurt = true;
            }
            // ��������ڵ����Ҳ࣬���˵����Ҳ�
            else if (this.transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(4, rb.velocity.y);
                // ������Ч
                hurtAudio.Play();
                // ���˱��
                isHurt = true;
            }
        }
        
    }

    // �¶�
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
            }
            else
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }
        }
    }

    // ��ɫ��Ծ
    void Jump()
    {
        // ��ס��Ծ���ҽ�ɫ�Ӵ�����
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            // ��ɫԾ��
            rb.velocity = new Vector2(0, jumpForce);
            // ������Ч
            jumpAudio.Play();
            anim.SetBool("jumping", true);
        }
    }

    // ���õ�ǰ����
    void Restart()
    {
        // ���õ�ǰ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);      
    }

    // ӣ�Ҽ���
    public void CherryCount()
    {
        cherryCount += 1;
    }


}
