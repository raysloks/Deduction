using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulseCheckerUI : MonoBehaviour
{
    public TextMeshProUGUI myText;
    private bool coolingDown = false;
    private float counter;
    public GameObject parent;
    public GameObject playerTrigger;
    public GameController gc;
    // Start is called before the first frame update
    void Start()
    {
       // myText = GetComponent<TextMeshProUGUI>();

    }


    public void StartCountdown(float seconds)
    {
        if (!coolingDown)
        {
            playerTrigger.SetActive(true);
            parent.SetActive(true);
            StartCoroutine(Countdown(seconds));
        }
        else
        {
            counter = seconds;
        }
    }

    public IEnumerator Countdown(float seconds)
    {
         counter = seconds;

         coolingDown = true;
        gc.pulseActive = true;
        Debug.Log("Countdown");


         while (counter > 1)
         {
             myText.text = Mathf.Round(counter).ToString();
             counter -= Time.deltaTime;
             yield return null; 
         }


        playerTrigger.SetActive(false);
        parent.SetActive(false);


        gc.pulseActive = false;
        coolingDown = false;        
    }
}
