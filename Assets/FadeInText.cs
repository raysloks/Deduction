using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeInText : MonoBehaviour
{

    TextMeshProUGUI myText;
    // Start is called before the first frame update
    void Start()
    {

        myText = GetComponent<TextMeshProUGUI>();

        string slowText = myText.text;
        myText.text = "";
        StartCoroutine(fadeText(0.2f, slowText));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator fadeText(float sec, string slow)
    {
        for (int i = 0; i < slow.Length; i++)
        {
            myText.text = string.Concat(myText.text, slow[i]);
            //Wait a certain amount of time, then continue with the for loop
            yield return new WaitForSeconds(sec);
        }
    }
}
