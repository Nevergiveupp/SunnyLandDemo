using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void Death()
    {
        // ����PlayerController���е�ӣ�Ҽ�������
        FindObjectOfType<PlayerController>().CherryCount();
        Destroy(this.gameObject);
    }
}
