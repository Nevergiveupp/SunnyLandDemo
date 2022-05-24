using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;

    public Joystick joystick;

    // 掉落平台延迟时间
    public float waitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // 是否松开下键
        if (Input.GetButtonUp("Crouch") || joystick.Vertical >= -0.5f)
        {
            waitTime = 0.5f;
        }


        // 是否按住下键
        if (Input.GetButton("Crouch") || joystick.Vertical < -0.5f)
        {
            if (waitTime <= 0)
            {
                // 翻转单向平台
                effector.rotationalOffset = 180f;
                waitTime = 0.5f;
            } else
            {
                waitTime -= Time.deltaTime;
            }

        }

        if (Input.GetButton("Jump") || joystick.Vertical > 0.5f || JudgeJumpButton())
        {
            effector.rotationalOffset = 0;
        }
    }

    public bool JudgeJumpButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.name != "JumpButton")
            {
                Debug.Log("没有点击到按钮");
                return false;
            } else
            {
                return true;
            }
        } else
        {
            return false;
        }
    }
}
