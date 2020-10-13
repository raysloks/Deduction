﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;
using TMPro;

public class MeetingUiListener : MonoBehaviour
{
    public Canvas MeetingCanvas;
    public GameObject imagePrefab;
    private GameObject layoutGroup;
    private RadialLayout radialLayout;

    private Dictionary<ulong, GameObject> players = new Dictionary<ulong, GameObject>();

    public bool killOnTies = false;
    public bool enableSkipButton = true;

    private float circleSpeed = 1f;
    private int maxAngle;
    public int circleLayoutDistance = 215;
    private bool circleUpdate = false;
    private bool meetingDone = false;
    private bool firstMeeting = true;
    public int youCanVoteTimes = 2;
    private NetworkHandler handler;
    private GameObject skipButton;
    // Start is called before the first frame update
    void Start()
    {
        if(MeetingCanvas != null)
        {
            layoutGroup = MeetingCanvas.gameObject.transform.GetChild(0).gameObject;
            radialLayout = layoutGroup.GetComponent<RadialLayout>();
            skipButton = MeetingCanvas.gameObject.transform.GetChild(1).gameObject;
        }
        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_STARTED, MeetingStarted);
     ///   EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_ENDED, MeetingDone);
      //  EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_CHECKVOTES, checkVotes);

        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_VOTED, vote);


    }

    // Update is called once per frame
    void Update()
    {
        if(circleUpdate == true)
        {
            radialLayout.MaxAngle += 1f * Time.deltaTime * circleSpeed;
            if(radialLayout.MaxAngle >= maxAngle)
            {
                radialLayout.MaxAngle = maxAngle;
                circleUpdate = false;
            }
        }
       
    }

    void MeetingStarted(EventCallbacks.Event eventInfo)
    {
        
        if (MeetingCanvas == null)
        {
            return;
        }
        MeetingCanvas.gameObject.SetActive(true);
        if (enableSkipButton == true)
        {
            skipButton.gameObject.SetActive(true);
            skipButton.GetComponent<VoteButton>().amountVoted = 0;
          //  skipButton.GetComponent<VoteButton>().myText.text = "0";

        }
        else
        {
            skipButton.gameObject.SetActive(false);
        }

        radialLayout.fDistance = circleLayoutDistance;

        MeetingEvent meetingEvent = (MeetingEvent)eventInfo;
        handler = meetingEvent.meetingHandler;
        GameObject newObj;

        int amountOfPlayersAlive = 0;
        if(firstMeeting == true)
        {
            foreach (KeyValuePair<ulong, Mob> n in handler.mobs)
            {

                if (n.Value.IsAlive == true)
                {
                    amountOfPlayersAlive++;
                    newObj = (GameObject)Instantiate(imagePrefab, layoutGroup.transform);
                    newObj.GetComponent<Image>().sprite = n.Value.sprite.sprite;
                    if (handler.names.ContainsKey(n.Key))
                    {
                        Debug.Log("THIS IS THE NAME " + handler.names[n.Key]);
                        newObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = handler.names[n.Key];
                        newObj.name = handler.names[n.Key];
                    }
                    newObj.transform.SetParent(layoutGroup.transform);
                    players.Add(n.Key, newObj);
                }
            }
            firstMeeting = false;
        }
        else
        {
            foreach (KeyValuePair<ulong, GameObject> go in players)
            {
                if (handler.mobs.ContainsKey(go.Key))
                {
                    if (handler.mobs[go.Key].IsAlive == true)
                    {
                        go.Value.GetComponent<VoteButton>().amountVoted = 0;
                        go.Value.GetComponent<VoteButton>().myText.text = "0";
                        go.Value.SetActive(true);
                    }
                    else
                    {
                        go.Value.SetActive(false);
                        players.Remove(go.Key);
                    }
                }
            }
        }
       

        //change angle of layout based on number of players;
        switch (amountOfPlayersAlive) {
            case 1:
                maxAngle = 0; break;
            case 2:
                maxAngle = 180; break;
            case 3:
                maxAngle = 240; break;
            case 4:
                maxAngle = 270; break;
            case 5:
                maxAngle = 287; break;
            case 6:
                maxAngle = 300; break;
            case 7:
                maxAngle = 308; break;
            case 8:
                maxAngle = 315; break;
            case 9:
                maxAngle = 320; break;
            case 10:
                maxAngle = 323; break;
        }

        radialLayout.MaxAngle = maxAngle;


        circleUpdate = false;
        meetingDone = false;



    }
    void vote(EventCallbacks.Event eventInfo)
    {

        VoteEvent voteEvent = (VoteEvent)eventInfo;
        ulong voterId = voteEvent.idOfVoter;
        Debug.Log("Vote for: " + voteEvent.nameOfButton);
        youCanVoteTimes = voteEvent.totalAmountOfVotes;
        if (handler.mobs.ContainsKey(voterId - 1)){
            if (voteEvent.nameOfButton == skipButton.name)
            {
                skipButton.GetComponent<VoteButton>().voteExternal(handler.mobs[voterId - 1].sprite);
            }
            else
            {
                foreach (KeyValuePair<ulong, GameObject> go in players)
                {
                    if (voteEvent.nameOfButton == go.Value.name)
                    {
                        go.Value.GetComponent<VoteButton>().voteExternal(handler.mobs[voterId - 1].sprite);
                        break;
                    }

                }
            }
        }
        else
        {
            Debug.Log("Vote Failed With this id: " + voterId);
        }
       
        checkVotes();
    }

    void checkVotes()
    {
        int totalVotes = 0;
        
         foreach (KeyValuePair<ulong, GameObject> go in players)
         {
            totalVotes += go.Value.GetComponent<VoteButton>().amountVoted;
         }
         if(totalVotes >= (players.Count * youCanVoteTimes))
         {
            Debug.Log("This is bigger: " + totalVotes + " Than this " + (players.Count * youCanVoteTimes));
            MeetingDone();
         }
        else
        {
            Debug.Log("This is bigger: " + (players.Count * youCanVoteTimes) + " Than this " + totalVotes);
        }
        
    }

    void MeetingDone()
    {
        if (MeetingCanvas == null)
        {
            return;
        }
        List<ulong> ties = new List<ulong>();
        int mostVotes = 0;
        foreach (KeyValuePair<ulong, GameObject> go in players)
        {
            int checkVotes = go.Value.GetComponent<VoteButton>().amountVoted;
            if (checkVotes >= mostVotes)
            {
                if(checkVotes == mostVotes)
                {
                    ties.Add(go.Key);
                }
                else
                {
                    ties.Clear();
                    ties.Add(go.Key);
                }
                mostVotes = checkVotes;
            }
            if(ties.Count > 1 && killOnTies == true)
            {
                foreach(ulong l in ties)
                {
                    Debug.Log("Remove multiple" + l);

                    MobRemoved message = new MobRemoved
                    {
                        id = (l),
                    };
                    handler.link.Send(message);
                }
            }
            else if(ties.Count == 1)
            {
                ulong id2 = ties[0];
                Debug.Log("Remove this" + id2 + " name: " + handler.names[id2]);
                MobRemoved message = new MobRemoved
                {
                    id = (id2),
                };
                handler.link.Send(message);
                
            }
           
            if (handler.mobs.ContainsKey(go.Key))
            {
                if (handler.mobs[go.Key].IsAlive == true)
                {
                    go.Value.GetComponent<VoteButton>().amountVoted = 0;
                    go.Value.GetComponent<VoteButton>().myText.text = "0";
                    go.Value.SetActive(true);
                }
                else
                {
                    go.Value.SetActive(false);
                    players.Remove(go.Key);
                }
            }
        }

        MeetingCanvas.gameObject.SetActive(false);

        meetingDone = true;

    }
}
