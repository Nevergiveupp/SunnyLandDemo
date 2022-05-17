using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;

    // ����ƽ̨�ӳ�ʱ��
    public float waitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // �Ƿ��ɿ��¼�
        if (Input.GetButtonUp("Crouch"))
        {
            waitTime = 0.5f;
        }


        // �Ƿ�ס�¼�
        if (Input.GetButton("Crouch"))
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

        if (Input.GetButton("Jump"))
        {
            effector.rotationalOffset = 0;
        }
    }
}
