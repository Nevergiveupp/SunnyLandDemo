using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    private float playerGravity;
    private float vertical;
    private float speed = 6f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Animator anim;

    public Joystick joystick;

    void Start()
    {
        // 获取初始重力
        playerGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");//[-1, 1]
        // 适配手机
        vertical = vertical == 0 ? joystick.Vertical : vertical;

        // 角色接触梯子，且按下上或下键
        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
            // 切换攀爬动画
            anim.SetBool("climbing", true);
        }
    }

    // working with physics using FixedUpdate
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = playerGravity;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            // 切换攀爬动画
            anim.SetBool("climbing", false);
        }

    }
}
