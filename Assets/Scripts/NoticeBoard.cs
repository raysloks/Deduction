using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticeBoard : MonoBehaviour
{
    public float speed = 500f;
    public float slowedDownSpeed = 10f;
    private bool MoveNoticedBoard = false;
    private TextMeshProUGUI myText;
    private Image myImage;
    private Image myImage2;

    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 middlePos;

    // Start is called before the first frame update
    void Start()
    {
        myText = this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        myImage = this.gameObject.transform.GetChild(1).GetComponent<Image>();
        myImage2 = this.gameObject.transform.GetChild(2).GetComponent<Image>();

        startPos = transform.position;
        endPos = this.gameObject.transform.GetChild(3).transform.position;
        middlePos = this.gameObject.transform.GetChild(4).transform.position;

    }


    // Update is called once per frame
    void Update()
    {
        // -1578, -28
        if (MoveNoticedBoard)
        {
            float step;
            if (Vector2.Distance(transform.position, middlePos) < 15f)
            {
                step = slowedDownSpeed * Time.deltaTime;
            }
            else
            {
                step = speed * Time.deltaTime;

            }
            transform.position = Vector2.MoveTowards(transform.position, endPos, step);
           
            if (Vector2.Distance(transform.position, endPos) < 0.001f)
            {
                MoveNoticedBoard = false;
            }
        }
    }

    public void MoveTheBoard(string text, Sprite initiator, Sprite body)
    {
        transform.position = startPos;
        myText.text = text;
        if(initiator != null)
        {
            myImage.enabled = true;
            myImage.sprite = initiator;
        }
        else
        {
            myImage.enabled = false;
        }

        if(body != null)
        {
            myImage2.enabled = true;
            myImage2.sprite = body;
        }
        else
        {
            myImage2.enabled = false;
        }
        MoveNoticedBoard = true;
    }
}
