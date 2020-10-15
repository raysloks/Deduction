using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;


public class VoteButton : MonoBehaviour
{


    public float speedOfDisaperance = 10f;
    public AudioClip soundWhenVotedOut;
    public GameObject particleWhenVotedOut;
    public Material m;


    private Button myButton;
    private Image myImage;
    private Image mySecondaryImg;
    private Image myVotedImg;
    private TMP_Text myName;


    private Color colorWhite = Color.white;
    private Color lerpedColor = Color.white;

    private List<SpriteRenderer> sr = new List<SpriteRenderer>();
  
    private bool disapering = false;

    [HideInInspector] public TMP_Text myText;
    [HideInInspector] public int amountVoted = 0;
    [HideInInspector] public int amountVotedinternal = 0;



    private float timer = 0;
    private bool timerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        myButton = this.GetComponent<Button>();
      //  myName = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        myName = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        myText = this.gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        mySecondaryImg = this.gameObject.transform.GetChild(2).gameObject.GetComponent<Image>();
        myVotedImg = this.gameObject.transform.GetChild(3).gameObject.GetComponent<Image>();

        //  myButton.onClick.AddListener(delegate { vote(); });
        //  EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_VOTED, vote);
        m.SetFloat("_ShakeUvSpeed", 0f);
        m.SetFloat("_ShakeUvX", 0f);
        m.SetFloat("_ShakeUvY", 0f);

        m.SetFloat("_NegativeAmount", 0f);
        m.SetFloat("_PinchUvAmount", 0f);
        m.SetFloat("_HitEffectBlend", 0f);

    }

    void Update()
    {
        if (timerOn == true)
        {
            timer += 0.2f * Time.deltaTime;
            if (timer < 1f)
            {
                myImage.material.SetFloat("_ShakeUvSpeed", timer * 20f);
                myImage.material.SetFloat("_ShakeUvX", timer * 5f);
                myImage.material.SetFloat("_ShakeUvY", timer * 5f);

             //   myImage.material.SetFloat("_NegativeAmount", timer * 1f);
                myImage.material.SetFloat("_PinchUvAmount", timer * 0.5f);
                myImage.material.SetFloat("_HitEffectBlend", timer * 0.5f);
            }
            else
            {
                MeetingDieEvent mde = new MeetingDieEvent();
                mde.UnitGameObjectPos = this.gameObject.transform.position;
                mde.UnitSound = soundWhenVotedOut;
                mde.UnitParticle = particleWhenVotedOut;
                

                EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_DIED, mde);

                myImage.enabled = false;
                mySecondaryImg.enabled = false;
                myText.enabled = false;
                myName.enabled = false;

                timerOn = false;

                Debug.Log("Done");
            }
        }


        if (sr.Count > 0 && disapering == false)
        {
            amountVotedinternal++;
            myText.text = amountVotedinternal.ToString();

            mySecondaryImg.sprite = sr[0].sprite;
            //    colorWhite = mySecondaryImg.color;

            colorWhite.a = 1f;
            lerpedColor = colorWhite;
            lerpedColor.a = 0f;
            mySecondaryImg.color = colorWhite;
            disapering = true;
        }
        else if (disapering == true)
        {
            colorWhite = Color.Lerp(colorWhite, lerpedColor, 3f * Time.deltaTime);
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

        amountVoted++;
        if (amountVoted == 1)
        {
            amountVotedinternal = 0;
        }
        sr.Add(votedSprite);

        //   myText.text = amountVoted.ToString();
    }



    void vote(EventCallbacks.Event Eventinfo)
    {
        VoteEvent votingEvent = (VoteEvent)Eventinfo;
        if (votingEvent.nameOfButton == this.gameObject.name)
        {
            Debug.Log("Vote for: " + votingEvent.nameOfButton);
            amountVoted++;
            myText.text = amountVoted.ToString();
            EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_CHECKVOTES, votingEvent);

        }

    }
    public void setText(string t)
    {
        myText.text = t;
    }
    public void setMaterial()
    {
        //   this.gameObject.GetComponent<AllIn1Shader>().MakeCopy();
        myImage = this.GetComponent<Image>();
        myImage.material = m;
        myVotedImg.material = m;
        this.gameObject.GetComponent<AllIn1Shader>().ToggleSetAtlasUvs(true);
        timerOn = true;
        if (myImage != null)
        {
            myImage.material.SetFloat("_ShakeUvSpeed", 20f);
            myImage.material.SetFloat("_ShakeUvX", 5f);
            myImage.material.SetFloat("_ShakeUvY", 5f);

            myImage.material.SetFloat("_NegativeAmount", 1f);
            myImage.material.SetFloat("_PinchUvAmount", 0.5f);
            myImage.material.SetFloat("_HitEffectBlend", 0.7f);


            // myImage.material.SetFloat("_Pinch", 0.5f);

        }
        else
        {
            Debug.Log("imageGONE");
        }
    }
}
