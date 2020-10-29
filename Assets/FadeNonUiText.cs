using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FadeNonUiText : MonoBehaviour
{

    TextMeshPro myText;
    Color c;
    Color goal;
    Color lerpedColor;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshPro>();
        c = myText.color;
        goal = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(FadeIn(3));
    }

    IEnumerator FadeIn(float Sec)
    {
        float counter = Sec;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            lerpedColor = Color.Lerp(goal, c, counter);
            myText.color = lerpedColor;
            yield return null;
        }
    }
}
