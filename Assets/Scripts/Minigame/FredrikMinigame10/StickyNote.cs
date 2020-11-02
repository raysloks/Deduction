using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StickyNote : MonoBehaviour
{
    public GameObject bg;
    private SpriteRenderer sr;
    TextMeshPro myText;
    Color goal;
    Color goal2;
    Color c;
    Color lerpedColor;
    private bool doneFading = true;
    // Start is called before the first frame update
    void Start()
    {
        sr = bg.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        myText = transform.GetChild(1).GetComponent<TextMeshPro>();

        c = myText.color;
        goal = new Color(myText.color.r, myText.color.g, myText.color.b, 255f);
        goal2 = new Color(myText.color.r, myText.color.g, myText.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        if (doneFading)
        {

            sr.enabled = true;
            myText.color = goal;
            doneFading = false;
            StartCoroutine(fadeText(3));
        }
    }
    IEnumerator fadeText(float sec)
    {
        float counter = sec;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            lerpedColor = Color.Lerp(goal2, goal, counter);
            myText.color = lerpedColor;
            yield return null;
        }
        sr.enabled = false;

        doneFading = true;
    }
}
