using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StickyNote : Interactable
{
    public GameObject bg;

    public SpriteRenderer outline;
    private SpriteRenderer sr;
    private SpriteRenderer sr2;
    TextMeshPro myText;
    Color goal;
    Color goal2;
    Color c;
    Color lerpedColor;
    private bool doneFading = true;
    private TextMeshProUGUI texty;

    void Start()
    {
    //    sr = bg.GetComponent<SpriteRenderer>();
        sr2 = GetComponent<SpriteRenderer>();
      //  sr.enabled = false;
        myText = transform.GetChild(1).GetComponent<TextMeshPro>();

        texty = bg.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        c = myText.color;
        goal = new Color(myText.color.r, myText.color.g, myText.color.b, 255f);
        goal2 = new Color(myText.color.r, myText.color.g, myText.color.b, 0f);
    }

    public override bool CanInteract(GameController game)
    {
        return true;
    }

    public override void Interact(GameController game)
    {
        Debug.Log("Interact");
        texty.text = myText.text;
        game.popup.ActivatePopup(bg, null);
    }

    public override void Highlight(bool highlighted)
    {
        if (outline != null)
        {

            Debug.Log("FadeIn sticky");
            if (highlighted)
                StartCoroutine(FadeInOutline(0.2f));
            else
                StartCoroutine(FadeOutOutline(0.2f));
        }
    }

    IEnumerator fadeText(float sec, string slow)
    {
        for (int i = 0; i < slow.Length; i++)
        {
            myText.text = string.Concat(myText.text, slow[i]);
            //Wait a certain amount of time, then continue with the for loop
            yield return new WaitForSeconds(sec);
        }
        float wait = 5f;
        yield return new WaitForSeconds(wait);
        sr.enabled = false;
        myText.color = goal2;
        doneFading = true;
    }

    IEnumerator FadeInOutline(float seconds)
    {
        float counter = 0;

        while (counter < seconds)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOutOutline(float seconds)
    {
        float counter = seconds;

        while (counter > 0f)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter -= Time.deltaTime;
            yield return null;
        }

    }
}
