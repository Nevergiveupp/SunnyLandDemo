using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;

    // 掉落平台延迟时间
    public float waitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // 是否松开下键
        if (Input.GetButtonUp("Crouch"))
        {
            waitTime = 0.5f;
        }


        // 是否按住下键
        if (Input.GetButton("Crouch"))
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

        if (Input.GetButton("Jump"))
        {
            effector.rotationalOffset = 0;
        }
    }
}
