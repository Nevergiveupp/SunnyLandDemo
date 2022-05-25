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

    // ��Ч��ײ��
    [Tooltip("Player Collider")]
    public Collider2D coll;

    // ͷ����ײ��
    public Collider2D headColl;
    // �¶���ײ��
    public Collider2D crouchColl;

    // ͷ��/�������
    public Transform cellingCheck, groundCheck;

    //[Tooltip("Jump Audio")]
    //public AudioSource jumpAudio;

    //[Tooltip("Hurt Audio")]
    //public AudioSource hurtAudio;

    //[Tooltip("Get Item Audio")]
    //public AudioSource getItemAudio;

    [Tooltip("Player Move Speed")]
    public float speed;

    [Tooltip("Player Jump Force")]
    public float jumpForce;

    //[Tooltip("Player Climb Speed")]
    //public float climbSpeed;

    [Tooltip("Layer")]
    public LayerMask ground;
    public LayerMask ladder;

    [SerializeField]
    public int cherryCount, gemCount;

    [SerializeField]
    public TMP_Text cherryNum, gemNum, totalScore;

    // �����Ƿ����ˣ�Ĭ��false
    private bool isHurt;

    // �Ƿ��ڵ��棬Ĭ��false
    private bool isGround;
    // ������Ծֵ��Ĭ��0
    private int extraJump;

    [Tooltip("Player MaxHealth")]
    public int maxHealth = 100;
    [Tooltip("Player CurrentHealth")]
    public int currentHealth;

    public HealthBar healthBar;

    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public GameObject passMenu;

    public bool isOnFire = false;

    public Joystick joystick;

    public GameObject fireButton;

    // Start is called before the first frame update
    void Start()
    {
        // �ȶ�֡��Ϊ60
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // ��ʼ��Ѫ��
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
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
        // ���ÿһ֡��ɫ�Ƿ��ڵ����������
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, ladder);
    }

    // Update is called once per frame
    private void Update()
    {
        // ��ɫ��Ծ
        //Jump();
        // ��ɫ�¶�
        Crouch();
        cherryNum.text = StaticCount.cherryCount.ToString();
        gemNum.text = StaticCount.gemCount.ToString();
        newJump();
        
    }

    // ��ɫ�ƶ�
    void Movement()
    {
        // ��ȡˮƽ���ֵ���洢�ڱ�����
        float horizontalValue = Input.GetAxis("Horizontal");
        // ���Ի�ȡҡ��
        horizontalValue = horizontalValue == 0 ? joystick.Horizontal : horizontalValue;// -1f~1f
        // ��ɫ�泯����
        float faceDirection = Input.GetAxisRaw("Horizontal");
        faceDirection = faceDirection == 0 ? joystick.Horizontal : faceDirection;

        float move = horizontalValue * speed * Time.fixedDeltaTime;

        // ��ɫ�ƶ�
        if (horizontalValue != 0)
        {
            // ���ý�ɫ�ٶ�
            rb.velocity = new Vector2(move, rb.velocity.y); // ע�⣺FixUpdate()������ʹ��Time.fixedDeltaTime����ʹ���������
            // ���ö�������
            anim.SetFloat("running", Mathf.Abs(faceDirection));
        }

        // ��ɫ����
        //if (faceDirection != 0)
        //{
        //    this.transform.localScale = new Vector3(faceDirection, 1, 1);
        //}

        // ����ת����Է�תx�ᣬ����ӵ�����һֱ���ҵ�����
        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
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
        else if (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(ladder))
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
            //getItemAudio.Play();
            SoundManager.instance.GetItemAudio();
            // ���ٵ�ǰ����ײ����
            //Destroy(collider.gameObject);
            // ������һ
            //cherryCount += 1;
            // ������ײ�嶯��
            Debug.Log(collider.name);
            if (collider.name.Contains("Cherry"))
            {
                collider.GetComponent<Animator>().Play("IsGot");
            }
            if (collider.name.Contains("Gem"))
            {
                collider.GetComponent<Animator>().Play("GemIsGot");
            }
            //cherryNum.text = cherryCount.ToString();
        }

        // ��ɫ����
        if (collider.tag == "DeadLine")
        {
            // ����������Դ
            // this.GetComponent<AudioSource>().enabled = false;
            ClearCollectionNum();
            // �ӳ�ִ�����õ�ǰ����
            Invoke("Restart", 1f);

        }

        if (collider.tag == "Finish")
        {
            GamePass();
            
            isOnFire = false;
        }

        // ���Է������
        if (collider.tag == "Item")
        {
            isOnFire = true;

            fireButton.SetActive(true);
            Destroy(collider.gameObject);
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
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                // ���˱��
                isHurt = true;
                // Ѫ������
                TakeDamage(25);
            }
            // ��������ڵ����Ҳ࣬���˵����Ҳ�
            else if (this.transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(4, rb.velocity.y);
                // ������Ч
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                // ���˱��
                isHurt = true;
                // Ѫ������
                TakeDamage(25);
            }
        }
        
    }

    // �¶�
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.5f, ground))
        {
            if (Input.GetButton("Crouch") || joystick.Vertical < -0.5f)
            {
                anim.SetBool("crouching", true);
                headColl.enabled = false;
            }
            else
            {
                anim.SetBool("crouching", false);
                headColl.enabled = true;
                
            }
        }
    }

    // ��ɫ��Ծ
    /**void Jump()
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
    }*/

    // �Ż���Ծ�ָ�
    public void newJump()
    {
        if (isGround)
        {
            // ��������������1��
            extraJump = 1;
        }
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            // ������
            rb.velocity = Vector2.up * jumpForce;// new Vector2 (0, 1)
            // ������Ч
            //jumpAudio.Play();
            // ������Ծ����һ
            extraJump--;
            // ���������������е���Ծ��Ч
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
        if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
            // ���������������е���Ծ��Ч
            SoundManager.instance.JumpAudio();
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

    // ��ʯ����
    public void GemCount()
    {
        gemCount += 1;
    }

    public void TakeDamage(int damage)
    {
        // �ۼ�Ѫ��
        currentHealth -= damage;
        // ����Ѫ��
        healthBar.SetHealth(currentHealth);
        // ����
        if(currentHealth <= 0)
        {
            ClearCollectionNum();
            // ������Ϸ
            Invoke("Restart", 1f);
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        transform.localScale = flipped;

        transform.Rotate(0f, 180f, 0f);
    }

    void GamePass()
    {
        
        // ����ͨ�ضԻ���ʱ��ֹͣ
        passMenu.SetActive(true);
        totalScore.text = (StaticCount.cherryCount * 10 + StaticCount.gemCount * 20).ToString();
        Debug.Log("totalScore: " + totalScore.text);
        Time.timeScale = 0f;

    }

    // �ռ�������
    void ClearCollectionNum()
    {
        StaticCount.cherryCount = 0;
        StaticCount.gemCount = 0;
    }

    public void AndriodJump()
    {
        if (isGround)
        {
            // ��������������1��
            extraJump = 1;
        }
        if (extraJump > 0)
        {
            // ������
            rb.velocity = Vector2.up * jumpForce;// new Vector2 (0, 1)
            // ������Ч
            //jumpAudio.Play();
            // ������Ծ����һ
            extraJump--;
            // ���������������е���Ծ��Ч
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
        if (extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
            // ���������������е���Ծ��Ч
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
    }

}



