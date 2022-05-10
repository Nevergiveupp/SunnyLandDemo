using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Player Transform")]
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // 相机跟随玩家移动
        this.transform.position = new Vector3(player.position.x, player.position.y, -10f);
    }
}
