using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StickyNote : MonoBehaviour
{
    public GameObject bg;
    private SpriteRenderer sr;
    private SpriteRenderer sr2;
    TextMeshPro myText;
    Color goal;
    Color goal2;
    Color c;
    Color lerpedColor;
    private bool doneFading = true;

    void Start()
    {
        sr = bg.GetComponent<SpriteRenderer>();
        sr2 = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        myText = transform.GetChild(1).GetComponent<TextMeshPro>();

        c = myText.color;
        goal = new Color(myText.color.r, myText.color.g, myText.color.b, 255f);
        goal2 = new Color(myText.color.r, myText.color.g, myText.color.b, 0f);
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (doneFading)
        {
            sr.enabled = true;
            myText.color = goal;
            string slowText = myText.text;
            myText.text = "";
            doneFading = false;
            StartCoroutine(fadeText(0.2f, slowText));
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

    void OnMouseEnter()
    {
        sr2.material.SetFloat("_WaveAmount", 10f);
        sr2.material.SetFloat("_WaveSpeed", 10f);
        sr2.material.SetFloat("_WaveStrenght", 10f);
        sr2.material.SetFloat("_HitEffectBlend", 0.5f);
    }

    void OnMouseExit()
    {
        sr2.material.SetFloat("_WaveAmount", 0f);
        sr2.material.SetFloat("_WaveSpeed", 0f);
        sr2.material.SetFloat("_WaveStrenght", 0f);
        sr2.material.SetFloat("_HitEffectBlend", 0f);
    }
}
