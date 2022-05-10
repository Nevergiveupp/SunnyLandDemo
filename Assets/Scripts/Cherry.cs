using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void Death()
    {
        // 调用PlayerController类中的樱桃计数方法
        FindObjectOfType<PlayerController>().CherryCount();
        Destroy(this.gameObject);
    }
}
