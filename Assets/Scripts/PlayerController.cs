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

    // 下半身碰撞体
    [Tooltip("Player Collider")]
    public Collider2D coll;

    // 上半身碰撞体（下蹲时关闭）
    public Collider2D disColl;

    // 头顶检查点
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

    // 人物是否受伤，默认false
    private bool isHurt;


    // Start is called before the first frame update
    void Start()
    {
        // 稳定帧率为60
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
    }

    // Update is called once per frame
    private void Update()
    {
        // 角色跳跃
        Jump();
        // 角色下蹲
        Crouch();
        cherryNum.text = cherryCount.ToString();
    }

    // 角色移动
    void Movement()
    {
        // 获取水平轴的值并存储在变量中
        float horizontalValue = Input.GetAxis("Horizontal");
        // 角色面朝方向
        float faceDirection = Input.GetAxisRaw("Horizontal");

        // 角色移动
        if (horizontalValue != 0)
        {
            // 设置角色速度
            rb.velocity = new Vector2(horizontalValue * speed * Time.fixedDeltaTime, rb.velocity.y); // 注意：FixUpdate()方法中使用Time.fixedDeltaTime可以使画面更流畅
            // 设置动画参数
            anim.SetFloat("running", Mathf.Abs(faceDirection));
        }

        // 角色朝向
        if (faceDirection != 0)
        {
            this.transform.localScale = new Vector3(faceDirection, 1, 1);
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
        else if (coll.IsTouchingLayers(ground))
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
            getItemAudio.Play();
            // 销毁当前被碰撞物体
            //Destroy(collider.gameObject);
            // 计数加一
            //cherryCount += 1;
            // 调用碰撞体动画
            collider.GetComponent<Animator>().Play("IsGot");
            //cherryNum.text = cherryCount.ToString();
        }

        // 角色死亡
        if (collider.tag == "DeadLine")
        {
            // 禁用所有音源
            this.GetComponent<AudioSource>().enabled = false;
            // 延迟执行重置当前场景
            Invoke("Restart", 2f);

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
                hurtAudio.Play();
                // 受伤标记
                isHurt = true;
            }
            // 如果人物在敌人右侧，受伤弹回右侧
            else if (this.transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(4, rb.velocity.y);
                // 播放音效
                hurtAudio.Play();
                // 受伤标记
                isHurt = true;
            }
        }
        
    }

    // 下蹲
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

    // 角色跳跃
    void Jump()
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


}
