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
    Color lerpedColor = Color.white;
    private Image mySecondaryImg;
    private bool disapering = false;
    public float speedOfDisaperance = 10f;
    [HideInInspector]public TMP_Text myText;

    [HideInInspector]public int amountVoted = 0;
    // Start is called before the first frame update
    void Start()
    {
        myButton = this.GetComponent<Button>();
        myText = this.gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        mySecondaryImg = this.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>();
        //  myButton.onClick.AddListener(delegate { vote(); });
        //     EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_VOTED, vote);


    }
    void Update()
    {
        if (sr.Count > 0 && disapering == false)
        {
            Debug.Log("Img can be seen");
            mySecondaryImg.sprite = sr[0].sprite;
            //    colorWhite = mySecondaryImg.color;
            
            colorWhite.a = 1f;
            lerpedColor = colorWhite;
            lerpedColor.a = 0f;
            mySecondaryImg.color = colorWhite;
            disapering = true;
        }
        else if(disapering == true)
        {
            Debug.Log("Alpha" + mySecondaryImg.color.a );
            //   float c = mySecondaryImg.color.a;
            //   colorWhite.a = c - (1f * speedOfDisaperance * Time.deltaTime);

            colorWhite = Color.Lerp(colorWhite, lerpedColor, 1f * Time.deltaTime);
            mySecondaryImg.color = colorWhite;


            if (0.01f >= mySecondaryImg.color.a)
            {
                colorWhite.a = 0f;
                mySecondaryImg.color = colorWhite;
                sr.RemoveAt(0);
                disapering = false;
            }
        }
    }
    public void voteExternal(SpriteRenderer votedSprite)
    {
        sr.Add(votedSprite);
        Debug.Log("sr added " + sr.Count);

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
