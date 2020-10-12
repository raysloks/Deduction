using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;


public class VoteButton : MonoBehaviour
{
        
    private Button myButton;
    private Color colorWhite = Color.white;
    private List<SpriteRenderer> sr = new List<SpriteRenderer>();

    private Image mySecondaryImg;
    private bool disapering;
    public float speedOfDisaperance = 2f;
    [HideInInspector]public TMP_Text myText;

    [HideInInspector]public int amountVoted = 0;
    // Start is called before the first frame update
    void Start()
    {
        myButton = this.GetComponent<Button>();
        myText = this.gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
        mySecondaryImg = this.gameObject.transform.GetChild(2).GetComponent<Image>();
        //  myButton.onClick.AddListener(delegate { vote(); });
        //     EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_VOTED, vote);


    }
    void update()
    {
        if (sr.Count > 0 && disapering == false)
        {            
            mySecondaryImg.sprite = sr[0].sprite;
            colorWhite.a = 255f;
            mySecondaryImg.color = colorWhite;
            disapering = true;
        }
        else if(disapering == true)
        {
            float c = mySecondaryImg.color.a;
            colorWhite.a = c - (1f * Time.deltaTime * speedOfDisaperance);
            mySecondaryImg.color = colorWhite;
            if(5f > mySecondaryImg.color.a)
            {
                colorWhite.a = 0f;
                mySecondaryImg.color = colorWhite;
                disapering = false;
            }
        }
    }
    public void voteExternal(SpriteRenderer votedSprite)
    {
        sr.Add(votedSprite);
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
