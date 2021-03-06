﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;
using TMPro;

public class MeetingUiListener : MonoBehaviour
{
    public RectTransform meetingPanel;
    public GameObject voteButtonPrefab;
    public GameObject voterPrefab;
    public NoticeBoard noticeBoard;
    public GameObject blackout;
    public GameObject evidenceScreen;
    public Button gavelButton;

    public RadialLayout radialLayout;
    public VoteButton skipButton;

    private Dictionary<ulong, VoteButton> voteButtons = new Dictionary<ulong, VoteButton>();

    public float fadeTime = 1f;

    public float circleLayoutDistance = 215f;
    public float circleSpeed = 120f;
    private float maxAngle;

    private GameController game;

    private void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_STARTED, MeetingStarted);
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
        //EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_CHECKVOTES, checkVotes);
        EventSystem.Current.RegisterListener(EVENT_TYPE.SEND_EVIDENCE, SendEvidence);
        EventSystem.Current.RegisterListener(EVENT_TYPE.PRESENT_EVIDENCE, PresentEvidence);

        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_VOTED, Vote);
    }

    private void Update()
    {
        //Circle effect done at the beginning of meetings
        if (radialLayout.MaxAngle != maxAngle)
        {
            radialLayout.MaxAngle = Mathf.MoveTowards(radialLayout.MaxAngle, maxAngle, Time.deltaTime * circleSpeed);
            radialLayout.StartAngle = 360f + radialLayout.MaxAngle - maxAngle;
            radialLayout.CalculateLayoutInputHorizontal();
        }
    }

    //Start the meeting. Add all vote buttons for each alive player
    private void MeetingStarted(EventCallbacks.Event eventInfo)
    {
        Debug.Log("Start Meeting");
        if (meetingPanel == null)
            return;

        meetingPanel.gameObject.SetActive(true);

        //When Meeting start Add all the evidence too the evidence screen
        evidenceScreen.GetComponent<EvidenceHandler>().AddAllEvidence();

        MeetingEvent me = (MeetingEvent)eventInfo;
        game = me.game;
        var handler = game.handler;
        if (me.EventDescription == "BodyReported")
            noticeBoard.MoveTheBoard("Found a Body", handler.mobs[me.idOfInitiator].sprite, handler.mobs[me.idOfBody].sprite);
        else
            noticeBoard.MoveTheBoard("Meeting Called by", null , handler.mobs[me.idOfInitiator].sprite);

        skipButton.gameObject.SetActive(game.settings.enableSkipButton);
        skipButton.game = game;
        skipButton.target = ulong.MaxValue;
        skipButton.ResetVoteCountAndState();

        gavelButton.interactable = true;

        radialLayout.fDistance = circleLayoutDistance;

        foreach (var n in voteButtons)
            Destroy(n.Value.gameObject);
        voteButtons.Clear();

        foreach (var n in handler.mobs)
        {
            Mob mob = n.Value;

            if (game.handler.names.ContainsKey(n.Key))
            {
                bool alive = mob.IsAlive && mob.gameObject.activeSelf;

                if (!voteButtons.ContainsKey(n.Key))
                {
                    GameObject go = Instantiate(voteButtonPrefab, radialLayout.transform);
                    VoteButton voteButton = go.GetComponent<VoteButton>();
                    voteButton.GetComponentInChildren<VoiceIndicatorImage>().voicePlayer = mob.GetComponentInChildren<VoicePlayer>();
                    voteButton.game = game;
                    voteButton.target = n.Key;
                    voteButton.targetImage.sprite = mob.sprite.sprite;
                    voteButton.targetImage.color = mob.sprite.color;
                    if (handler.names.ContainsKey(n.Key))
                    {
                        voteButton.nameText.text = handler.names[n.Key];
                        go.name = handler.names[n.Key];
                    }
                    voteButtons.Add(n.Key, voteButton);
                }

                if (voteButtons.ContainsKey(n.Key))
                {
                    VoteButton voteButton = voteButtons[n.Key];
                    voteButton.deadIndicator.SetActive(!alive);
                    voteButton.targetImage.sprite = mob.sprite.sprite;
                    voteButton.targetImage.color = mob.sprite.color;
                    voteButton.ResetVoteCountAndState();
                }
            }
        }

        //change angle of layout based on number of players;

        int playerCount = radialLayout.transform.childCount;
        if (playerCount != 0)
            maxAngle = 360f - 360f / playerCount;

        radialLayout.MaxAngle = 0f;
    }

    private void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent phaseChangedEvent = (PhaseChangedEvent)eventInfo;
        switch (phaseChangedEvent.phase)
        {
            case GamePhase.Discussion:
                break;
            case GamePhase.Voting:
                skipButton.button.interactable = true;
                foreach (var n in voteButtons)
                    n.Value.button.interactable = true;
                break;
            case GamePhase.EndOfMeeting:
                skipButton.button.interactable = false;
                foreach (var n in voteButtons)
                    n.Value.button.interactable = false;
                break;
            case GamePhase.Ejection:
                meetingPanel.gameObject.SetActive(false);
                blackout.SetActive(true);
                break;
            default:
                meetingPanel.gameObject.SetActive(false);
                blackout.SetActive(false);
                break;
        }
    }

    //Event thats fired when someone votes
    private void Vote(EventCallbacks.Event eventInfo)
    {
        VoteEvent voteEvent = (VoteEvent)eventInfo;
        ulong voterId = voteEvent.idOfVoter;
        ulong targetId = voteEvent.idOfTarget;
        //Debug.Log(voterId + " voted for " + targetId);

        var handler = game.handler;

        if (voteButtons.ContainsKey(voterId))
            voteButtons[voterId].VoteCast();

        if (targetId == ulong.MaxValue)
        {
            skipButton.VoteReceived(voterPrefab, handler.mobs[voterId].sprite);
        }
        else if (voteButtons.ContainsKey(targetId))
        {
            if (voterId == ulong.MaxValue - 2)
            {
                voteButtons[targetId].confirmedIndicator.SetActive(true);
            }
            else
            {
                voteButtons[targetId].VoteReceived(voterPrefab, handler.mobs[voterId].sprite);
            }
        }
    }

    //Sent from SendEvidenceHandler when sending evidence during meeting.
    //Sends the evidence info to the vote button of the player who made this evidence
    private void SendEvidence(EventCallbacks.Event eventInfo)
    {
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        voteButtons[see.idOfTarget].Evidence.GetComponent<VoterEvidence>().SetEvidence(eventInfo);
    }

    private void PresentEvidence(EventCallbacks.Event eventInfo)
    {
        PresentEvidenceEvent presentEvidenceEvent = (PresentEvidenceEvent)eventInfo;
        voteButtons[presentEvidenceEvent.presenter].Evidence.GetComponent<VoterEvidence>().SetEvidence(eventInfo);
    }
}
