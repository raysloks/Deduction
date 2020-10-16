using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;
using TMPro;

public class MeetingUiListener : MonoBehaviour
{
    public Canvas MeetingCanvas;
    public GameObject imagePrefab;
    public GameObject noticeBoardImage;
    
    private CanvasGroup csGrp;
    private GameObject skipButton;
    private GameObject layoutGroup;
    private RadialLayout radialLayout;

    private Dictionary<ulong, GameObject> players = new Dictionary<ulong, GameObject>();
    private List<ulong> ties = new List<ulong>();

    public int circleLayoutDistance = 215;
    private float circleSpeed = 1f;
    private int maxAngle;

    public bool killOnTies = false;
    public bool enableSkipButton = true;
    public bool showVotesWhenAllVoted = true;
    private bool circleUpdate = false;
    private bool meetingDone = false;
    private bool firstMeeting = true;
    private bool waitForDramaticEffect = false;
    private bool fadeAway = false;
    private bool fadeIn = false;
    private bool waitForAllTheVotes = false;
    private bool votesShown = false;


    private NetworkHandler handler;

    private float timer = 0f;
    private int youCanVoteTimes;
    private float fadeTime = 2f;
    private int index = 0;
    private ulong mostVotesId;



    // Start is called before the first frame update
    void Start()
    {
        if (MeetingCanvas != null)
        {
            layoutGroup = MeetingCanvas.gameObject.transform.GetChild(1).gameObject;
            radialLayout = layoutGroup.GetComponent<RadialLayout>();
            skipButton = MeetingCanvas.gameObject.transform.GetChild(2).gameObject;
            csGrp = MeetingCanvas.gameObject.GetComponent<CanvasGroup>();
        }
        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_STARTED, MeetingStarted);
        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_ENDED, MeetingDone);
        //  EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_CHECKVOTES, checkVotes);

        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_VOTED, vote);


    }

    // Update is called once per frame
    void Update()
    {
        //Fade in effect done at beginning of meetings
        if(fadeIn == true)
        {

            csGrp.alpha += Time.deltaTime / fadeTime;

            if (csGrp.alpha >= 1)
            {
                fadeIn = false;
            }
        }

        //Circle effect done at the beginning of meetings
        if (circleUpdate == true)
        {
            radialLayout.MaxAngle += 1f * Time.deltaTime * circleSpeed;
            if (radialLayout.MaxAngle >= maxAngle)
            {
                radialLayout.MaxAngle = maxAngle;
                circleUpdate = false;
            }
        }

        //Effect done at the end of the meeting
        if(meetingDone == true)
        {
            if (showVotesWhenAllVoted == true && votesShown == false)
            {
                timer += Time.deltaTime;
                if (timer > 2f)
                {
                    skipButton.GetComponent<VoteButton>().showAfterEveryoneVoted = false;
                    foreach (KeyValuePair<ulong, GameObject> go in players)
                    {
                        go.Value.GetComponent<VoteButton>().showAfterEveryoneVoted = false;
                    }
                    votesShown = true;
                    timer = 0;
                }

            }
            else if(ties.Count > 0 && fadeAway == false)
            {               
                    if (mostVotesId == (ulong)100)
                    {
                        if (skipButton.GetComponent<VoteButton>().done == true)
                        {
                            EndMeetingEffect();
                        }
                    }
                    else if (players[mostVotesId].GetComponent<VoteButton>().done == true)
                    {

                        EndMeetingEffect();
                    }
            }
            else if(fadeAway == false)
            {
                
                //if no one dies MAKE COOL TRANSITION
                timer += Time.deltaTime;
                if(timer > 4f)
                {
                    noticeBoardImage.GetComponent<NoticeBoard>().MoveTheBoard("No one was voted out", null, null);
                    fadeAway = true;
                    timer = 0;
                }
            }

            if (fadeAway == true )
            {

                csGrp.alpha -= Time.deltaTime;

                if (csGrp.alpha < 0.01f)
                {
                    csGrp.alpha = 0f;
                    RemoveMostVotedPlayer();

                    handler.controller.timerOn = true;
                    fadeAway = false;
                    meetingDone = false;
                    votesShown = false;


                }
            }


        }

    }

    void EndMeetingEffect()
    {
        if (waitForDramaticEffect == false && fadeAway == false)
        {
            timer += Time.deltaTime;
        }

        if (waitForDramaticEffect == false && timer > 4f && fadeAway == false)
        {
            Debug.Log("Dramatic Effect");

            foreach (ulong id in ties)
            {
                players[id].GetComponent<VoteButton>().setMaterial();
            }
            waitForDramaticEffect = true;
            timer = 0f;
        }
        else if (waitForDramaticEffect == true && fadeAway == false) ;
        {

            timer += Time.deltaTime;
            if (timer > 8f)
            {
                Debug.Log("Remove Effect");
                timer = 0f;
                fadeAway = true;
                waitForDramaticEffect = false;
                csGrp.alpha = 1;
            }
        }

       
    }
    //Start the meeting. Add all vote buttons for each alive player
    void MeetingStarted(EventCallbacks.Event eventInfo)
    {

        if (MeetingCanvas == null)
        {
            return;
        }
        MeetingCanvas.gameObject.SetActive(true);
        MeetingEvent meetingEvent = (MeetingEvent)eventInfo;
        handler = meetingEvent.meetingHandler;
        ulong initiator = meetingEvent.idOfInitiator;
        ulong body = meetingEvent.idOfBody;
        if(meetingEvent.EventDescription == "BodyReported")
        {
            noticeBoardImage.GetComponent<NoticeBoard>().MoveTheBoard("Found a Body", handler.mobs[initiator].sprite.sprite, handler.mobs[body].sprite.sprite);
        }
        else
        {
            noticeBoardImage.GetComponent<NoticeBoard>().MoveTheBoard("Meeting Started by", null , handler.mobs[initiator].sprite.sprite);
        }
        csGrp.alpha = 0;
        fadeIn = true;

        if (enableSkipButton == true)
        {
            skipButton.gameObject.SetActive(true);
            skipButton.GetComponent<VoteButton>().amountVoted = 0;
            skipButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "0";

        }
        else
        {
            skipButton.gameObject.SetActive(false);
        }

        radialLayout.fDistance = circleLayoutDistance;

        
        GameObject newObj;

        int amountOfPlayersAlive = 0;
        if (firstMeeting == true)
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
        switch (amountOfPlayersAlive)
        {
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

    //Event thats fired when someone votes
    void vote(EventCallbacks.Event eventInfo)
    {

        VoteEvent voteEvent = (VoteEvent)eventInfo;
        ulong voterId = voteEvent.idOfVoter;
        Debug.Log("Vote for: " + voteEvent.nameOfButton);
        youCanVoteTimes = voteEvent.totalAmountOfVotes;
       

        if (handler.mobs.ContainsKey(voterId))
        {
            if (voteEvent.nameOfButton == skipButton.name)
            {
                skipButton.GetComponent<VoteButton>().voteExternal(handler.mobs[voterId].sprite, showVotesWhenAllVoted);
            }
            else
            {
                foreach (KeyValuePair<ulong, GameObject> go in players)
                {
                    if (voteEvent.nameOfButton == go.Value.name)
                    {
                        go.Value.GetComponent<VoteButton>().voteExternal(handler.mobs[voterId].sprite, showVotesWhenAllVoted);
                        break;
                    }

                }
            }
        }
        else
        {
            Debug.Log("Vote Failed With this id: " + voterId);
        }

        if (voteEvent.doneVoting == true)
        {
            players[voterId].transform.GetChild(3).GetComponent<Image>().enabled = true;
        }
        checkVotes(eventInfo);
    }

    //Check if everyone has voted
    void checkVotes(EventCallbacks.Event eventInfo)
    {
        int totalVotes = 0;
        int AllRealPlayers = 0;
        foreach (KeyValuePair<ulong, GameObject> go in players)
        {
            string name = go.Value.name;
            if (name != "New Text" && name != "Player Name" && name != "VoteButton(Clone)" && name != "")
            {
                Debug.Log("One of the names: " + name);
                AllRealPlayers++;
            }
            totalVotes += go.Value.GetComponent<VoteButton>().amountVoted;
        }

        totalVotes += skipButton.GetComponent<VoteButton>().amountVoted;

        if (totalVotes >= (AllRealPlayers * youCanVoteTimes))
        {
            Debug.Log("This is bigger: " + totalVotes + " Than this " + (AllRealPlayers * youCanVoteTimes));
            handler.controller.timerOn = false;

            MeetingDone(eventInfo);
        }

    }
   
    
    //Check who has the most votes and put them into a list
    void MeetingDone(EventCallbacks.Event eventInfo)
    {
        Debug.Log("Meeting Done");
        int mostVotes = 0;
        foreach (KeyValuePair<ulong, GameObject> go in players)
        {
            int checkVotes = go.Value.GetComponent<VoteButton>().amountVoted;
            if (checkVotes >= mostVotes)
            {
                if (checkVotes == mostVotes)
                {

                    ties.Add(go.Key);
                }
                else
                {
                    mostVotesId = go.Key;
                    ties.Clear();
                    ties.Add(go.Key);
                }
                mostVotes = checkVotes;
            }
            if (enableSkipButton == true)
            {
                if (mostVotes < skipButton.GetComponent<VoteButton>().amountVoted)
                {
                    mostVotesId = (ulong)100;
                    ties.Clear();
                }
                if (mostVotes == skipButton.GetComponent<VoteButton>().amountVoted && killOnTies == false)
                {
                    ties.Clear();
                }
            }
        }
        if(ties.Count > 1 && killOnTies == false)
        {
            ties.Clear();
        }
        meetingDone = true;

    }

    //Kill most voted player. If killOnties is on it will kill all tied players otherwise it wont kill tied players
    void RemoveMostVotedPlayer()
    {
        if (ties.Count > 1 && killOnTies == true)
        {
            foreach (ulong l in ties)
            {
                Debug.Log("Remove multiple" + l);
                MobRemoved message = new MobRemoved
                {
                    id = (l),
                };
                handler.link.Send(message);
            }
        }
        else if (ties.Count == 1)
        {
            ulong id2 = ties[0];
            Debug.Log("Remove this" + id2 );
            MobRemoved message = new MobRemoved
            {
                id = (id2),
            };
            handler.link.Send(message);

        }
        else
        {
            Debug.Log("Noone died");
        }

        ties.Clear();


        //Reset vote text and remove dead players from menu**
        foreach (KeyValuePair<ulong, GameObject> go in players)
        {
            if (handler.mobs.ContainsKey(go.Key))
            {
                if (handler.mobs[go.Key].IsAlive == true)
                {
                    go.Value.GetComponent<VoteButton>().amountVoted = 0;
                    go.Value.GetComponent<VoteButton>().myText.text = "0";
                    go.Value.transform.GetChild(3).GetComponent<Image>().enabled = false;

                    go.Value.SetActive(true);
                }
                else
                {
                    go.Value.SetActive(false);
                    players.Remove(go.Key);
                }
            }
        }

        //Disable meeting canvas and change gamephase to main
        MeetingCanvas.gameObject.SetActive(false);
        changeGamePhase((ulong)1);
    }

    void changeGamePhase(ulong phase)
    {
        GamePhaseUpdate message = new GamePhaseUpdate
        {
            phase = phase
        };
        handler.link.Send(message);
    }
}
