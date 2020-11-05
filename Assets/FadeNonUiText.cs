using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FadeNonUiText : MonoBehaviour
{
    public float Faidsec = 3;
    public float WaitBeforeFaidsec = 0;
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
        StartCoroutine(FadeIn(Faidsec, WaitBeforeFaidsec));
    }
    

    IEnumerator FadeIn(float Sec, float sec2)
    {
        float counter = Sec;
        float counter2 = 0f;

        while (counter < sec2)
        {
            counter2 += Time.deltaTime;
            yield return null;
        }
        while (counter > 0)
        {
            counter -= Time.deltaTime;
            lerpedColor = Color.Lerp(goal, c, counter);
            myText.color = lerpedColor;
            yield return null;
        }
    }
}
