using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialog : MonoBehaviour
{
    public GameObject enterDialog;

    public GameObject enterButton;

    // ���Ӵ���ײ�����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (this.tag == "Door")
            {
                enterButton.SetActive(true);
            }
            
            enterDialog.SetActive(true);
        }

    }

    // ���뿪��ײ�����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (this.tag == "Door")
            {
                enterButton.SetActive(false);
            }
            enterDialog.SetActive(false);
        }
    }
}
