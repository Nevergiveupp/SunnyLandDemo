using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public void Death()
    {
        // 调用PlayerController类中的樱桃计数方法
        //FindObjectOfType<PlayerController>().GemCount();
        StaticCount.gemCount += 1;
        Destroy(this.gameObject);
    }

}
