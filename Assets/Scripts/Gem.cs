using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public void Death()
    {
        // ����PlayerController���е�ӣ�Ҽ�������
        //FindObjectOfType<PlayerController>().GemCount();
        StaticCount.gemCount += 1;
        Destroy(this.gameObject);
    }

}
