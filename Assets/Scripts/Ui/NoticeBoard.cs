using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticeBoard : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI myText;
    private Image myImage;
    private Image myImage2;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        myImage = transform.GetChild(1).GetComponent<Image>();
        myImage2 = transform.GetChild(2).GetComponent<Image>();
    }

    public void MoveTheBoard(string text, SpriteRenderer left, SpriteRenderer right)
    {
        myText.text = text;
        if (left != null)
        {
            myImage.enabled = true;
            myImage.sprite = left.sprite;
            myImage.color = left.color;
        }
        else
        {
            myImage.enabled = false;
        }

        if (right != null)
        {
            myImage2.enabled = true;
            myImage2.sprite = right.sprite;
            myImage2.color = right.color;
        }
        else
        {
            myImage2.enabled = false;
        }

        animator.enabled = true;
        animator.SetTrigger("Restart");
    }
}
