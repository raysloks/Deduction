using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;


public class VoteButton : MonoBehaviour
{
        
    private Button myButton;

    [HideInInspector]public TMP_Text myText;

    [HideInInspector]public int amountVoted = 0;
    // Start is called before the first frame update
    void Start()
    {
        myButton = this.GetComponent<Button>();
        myText = this.gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
      //  myButton.onClick.AddListener(delegate { vote(); });
   //     EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_VOTED, vote);


    }
    public void voteExternal()
    {
        amountVoted++;
        myText.text = amountVoted.ToString();
    }
    void vote(EventCallbacks.Event Eventinfo)
    {
        VoteEvent votingEvent = (VoteEvent)Eventinfo;
        if(votingEvent.nameOfButton == this.gameObject.name)
        {
            Debug.Log("Vote for: " + votingEvent.nameOfButton);
            amountVoted++;
            myText.text = amountVoted.ToString();
            EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_CHECKVOTES, votingEvent);
            
        }

    }
}
