using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialog : MonoBehaviour
{
    public GameObject enterDialog;

    public GameObject enterButton;

    // 当接触碰撞体调用
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

    // 当离开碰撞体调用
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
