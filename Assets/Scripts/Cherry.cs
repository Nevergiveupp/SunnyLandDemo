using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void Death()
    {
        // ����PlayerController���е�ӣ�Ҽ�������
        StaticCount.cherryCount += 1;
        //FindObjectOfType<PlayerController>().CherryCount();
        Destroy(this.gameObject);
    }
}
