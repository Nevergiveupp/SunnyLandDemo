using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Player Rigidbody")]
    //[SerializeField] // 使private变量在检查器中展示，debug用
    private Rigidbody2D rb;

    [Tooltip("Player Animator")]
    private Animator anim;

    // 生效碰撞体
    [Tooltip("Player Collider")]
    public Collider2D coll;

    // 头部碰撞体
    public Collider2D headColl;
    // 下蹲碰撞体
    public Collider2D crouchColl;

    // 头顶/地面检查点
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

    // 人物是否受伤，默认false
    private bool isHurt;

    // 是否在地面，默认false
    private bool isGround;
    // 额外跳跃值，默认0
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
        // 稳定帧率为60
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // 初始化血量
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    void FixedUpdate()
    {
        // 如果没受伤
        if (!isHurt)
        {
            // 执行角色动作
            Movement();
        }
        // 如果受伤，直接执行切换动画
        SwitchAnim();
        // 检测每一帧角色是否在地面或梯子上
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, ladder);
    }

    // Update is called once per frame
    private void Update()
    {
        // 角色跳跃
        //Jump();
        // 角色下蹲
        Crouch();
        cherryNum.text = StaticCount.cherryCount.ToString();
        gemNum.text = StaticCount.gemCount.ToString();
        newJump();
        
    }

    // 角色移动
    void Movement()
    {
        // 获取水平轴的值并存储在变量中
        float horizontalValue = Input.GetAxis("Horizontal");
        // 尝试获取摇杆
        horizontalValue = horizontalValue == 0 ? joystick.Horizontal : horizontalValue;// -1f~1f
        // 角色面朝方向
        float faceDirection = Input.GetAxisRaw("Horizontal");
        faceDirection = faceDirection == 0 ? joystick.Horizontal : faceDirection;

        float move = horizontalValue * speed * Time.fixedDeltaTime;

        // 角色移动
        if (horizontalValue != 0)
        {
            // 设置角色速度
            rb.velocity = new Vector2(move, rb.velocity.y); // 注意：FixUpdate()方法中使用Time.fixedDeltaTime可以使画面更流畅
            // 设置动画参数
            anim.SetFloat("running", Mathf.Abs(faceDirection));
        }

        // 角色朝向
        //if (faceDirection != 0)
        //{
        //    this.transform.localScale = new Vector3(faceDirection, 1, 1);
        //}

        // 这样转向可以翻转x轴，解决子弹发射一直向右的问题
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

    // 切换动画
    void SwitchAnim()
    {
        // 初始化idle
        //anim.SetBool("idle", false);

        // 解决从高处掉落时踩敌人会受伤的问题
        // 如果向上没有速度且不接触地面
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            // 角色在下落
            anim.SetBool("falling", true);
        }

        // 如果在跳跃
        if (anim.GetBool("jumping"))
        {
            // 如果在下落
            if(rb.velocity.y < 0)
            {
                // 设置动画为下落
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        // 如果角色受伤
        else if (isHurt)
        {
            // 动画受伤置为true
            anim.SetBool("hurt", true);
            // 动画running值置为0，解决受伤弹开后原地跑步的问题
            anim.SetFloat("running", 0);

            // 判断角色速度小于0.1，受伤状态结束
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                // 动画受伤完，受伤状态置为false，闲置状态置为true
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                // 重置为未受伤
                isHurt = false;
            }
        }

        // 如果角色接触地面
        else if (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(ladder))
        {
            // 设置动画为闲置
            anim.SetBool("falling", false);
            //anim.SetBool("idle", true);
        }
    }

    // 碰撞触发器
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 收集物品
        // 如果碰撞到Collections分类物体
        if (collider.tag == "Collections")
        {
            // 解决音效播放两次的问题
            collider.tag = "null";
            // 播放音效
            //getItemAudio.Play();
            SoundManager.instance.GetItemAudio();
            // 销毁当前被碰撞物体
            //Destroy(collider.gameObject);
            // 计数加一
            //cherryCount += 1;
            // 调用碰撞体动画
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

        // 角色死亡
        if (collider.tag == "DeadLine")
        {
            // 禁用所有音源
            // this.GetComponent<AudioSource>().enabled = false;
            ClearCollectionNum();
            // 延迟执行重置当前场景
            Invoke("Restart", 1f);

        }

        if (collider.tag == "Finish")
        {
            GamePass();
            
            isOnFire = false;
        }

        // 可以发射火球
        if (collider.tag == "Item")
        {
            isOnFire = true;

            fireButton.SetActive(true);
            Destroy(collider.gameObject);
        }
    }

    // 消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果碰撞到敌人
        if (collision.gameObject.tag == "Enemies")
        {
            // 获取敌人对象
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // 如果正在坠落
            if (anim.GetBool("falling"))
            {
                // 播放敌人死亡动画
                enemy.JumpOn();

                // 角色跃起
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("jumping", true);
            } 
            // 如果人物在敌人左侧，受伤弹回左侧
            else if (this.transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-4, rb.velocity.y);
                // 播放音效
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                // 受伤标记
                isHurt = true;
                // 血量减少
                TakeDamage(25);
            }
            // 如果人物在敌人右侧，受伤弹回右侧
            else if (this.transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(4, rb.velocity.y);
                // 播放音效
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                // 受伤标记
                isHurt = true;
                // 血量减少
                TakeDamage(25);
            }
        }
        
    }

    // 下蹲
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

    // 角色跳跃
    /**void Jump()
    {
        // 按住跳跃键且角色接触地面
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            // 角色跃起
            rb.velocity = new Vector2(0, jumpForce);
            // 播放音效
            jumpAudio.Play();
            anim.SetBool("jumping", true);
        }
    }*/

    // 优化跳跃手感
    public void newJump()
    {
        if (isGround)
        {
            // 二段跳，额外跳1次
            extraJump = 1;
        }
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            // 向上跳
            rb.velocity = Vector2.up * jumpForce;// new Vector2 (0, 1)
            // 播放音效
            //jumpAudio.Play();
            // 额外跳跃量减一
            extraJump--;
            // 调用声音管理器中的跳跃音效
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
        if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
            // 调用声音管理器中的跳跃音效
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
    }

    

    // 重置当前场景
    void Restart()
    {
        // 重置当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);      
    }

    // 樱桃计数
    public void CherryCount()
    {
        cherryCount += 1;
    }

    // 钻石计数
    public void GemCount()
    {
        gemCount += 1;
    }

    public void TakeDamage(int damage)
    {
        // 扣减血量
        currentHealth -= damage;
        // 设置血条
        healthBar.SetHealth(currentHealth);
        // 死亡
        if(currentHealth <= 0)
        {
            ClearCollectionNum();
            // 重置游戏
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
        
        // 弹出通关对话框，时间停止
        passMenu.SetActive(true);
        totalScore.text = (StaticCount.cherryCount * 10 + StaticCount.gemCount * 20).ToString();
        Debug.Log("totalScore: " + totalScore.text);
        Time.timeScale = 0f;

    }

    // 收集数清零
    void ClearCollectionNum()
    {
        StaticCount.cherryCount = 0;
        StaticCount.gemCount = 0;
    }

    public void AndriodJump()
    {
        if (isGround)
        {
            // 二段跳，额外跳1次
            extraJump = 1;
        }
        if (extraJump > 0)
        {
            // 向上跳
            rb.velocity = Vector2.up * jumpForce;// new Vector2 (0, 1)
            // 播放音效
            //jumpAudio.Play();
            // 额外跳跃量减一
            extraJump--;
            // 调用声音管理器中的跳跃音效
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
        if (extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
            // 调用声音管理器中的跳跃音效
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
    }

}



