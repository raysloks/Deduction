using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EventCallbacks;


public class PulseCheckerUI : MonoBehaviour
{
    //Displays pulse checker cooldown

    private bool coolingDown = false;
    private float counter;

    public TextMeshProUGUI myText;
    public GameObject parent;
    public GameObject playerTrigger;
    public GameController gc;

    // Start is called before the first frame update
    void Awake()
    {
        EventCallbacks.EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
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

    //End of meeting cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            if(coolingDown == true)
            {
                counter = 0;
                playerTrigger.SetActive(false);
                parent.SetActive(false);
                gc.pulseActive = false;
                coolingDown = false;

            }
        }
    }
}
