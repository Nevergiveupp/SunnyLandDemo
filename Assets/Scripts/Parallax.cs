using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Tooltip("Camera Transform")]
    public Transform cam;

    [Tooltip("Camera Move Speed")]
    public float moveRate;

    // ��ʼ��
    private float startPointX, startPointY;

    public bool lockY; // Ĭ��false

    // Start is called before the first frame update
    void Start()
    {
        startPointX = this.transform.position.x;
        startPointY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // y������
        if (lockY)
        {
            this.transform.position = new Vector2(startPointX + cam.position.x * moveRate, this.transform.position.y);
        }
        else
        {
            this.transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointY + cam.position.y * moveRate);
        }
        
    }
}
