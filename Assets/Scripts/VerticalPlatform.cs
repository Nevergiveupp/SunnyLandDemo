using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;

    public Joystick joystick;

    // ����ƽ̨�ӳ�ʱ��
    public float waitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // �Ƿ��ɿ��¼�
        if (Input.GetButtonUp("Crouch") || joystick.Vertical >= -0.5f)
        {
            waitTime = 0.5f;
        }


        // �Ƿ�ס�¼�
        if (Input.GetButton("Crouch") || joystick.Vertical < -0.5f)
        {
            if (waitTime <= 0)
            {
                // ��ת����ƽ̨
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
                Debug.Log("û�е������ť");
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
