using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class VoterEvidence : MonoBehaviour
{
    [HideInInspector] public enum Evidence { None, Picture, MotionSensor, SmokeGrenade, PulseChecker };
    [HideInInspector] public Evidence myEvidence;
    [HideInInspector] public byte[] ba;

    [HideInInspector] public MotionSensor ms;
    [HideInInspector] public SGEvidence sg;
    [HideInInspector] public PulseCheckerEvidence pc;

    [HideInInspector] public int photoIndex = -1;
    private GameController gc;
    public VoteButton vb;
    public GameObject newEvidence;

    // Start is called before the first frame update
    void Start()
    {
        myEvidence = Evidence.None;
        EventSystem.Current.RegisterListener(EVENT_TYPE.SNAPSHOT_EVIDENCE, SetEvidence2);
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }

    public void SetEvidence(EventCallbacks.Event eventInfo)
    {
        newEvidence.SetActive(true);
        if (eventInfo is SendEvidenceEvent see)
        {
            myEvidence = (Evidence)see.Evidence;
            if (myEvidence == Evidence.MotionSensor)
            {
                Debug.Log("MotionEvidence");
                ms = see.MotionSensorEvidence;
            }
            else if (myEvidence == Evidence.SmokeGrenade)
            {
                Debug.Log("SmokeEvidence");
                sg = see.smokeGrenadeEvidence;
            }
            else if (myEvidence == Evidence.PulseChecker)
            {
                Debug.Log("PulseEvidence");
                pc = see.pulseCheckerEvidence;
            }
        }
        if (eventInfo is PresentEvidenceEvent presentEvidenceEvent)
        {
            myEvidence = Evidence.Picture;
            photoIndex = presentEvidenceEvent.index;
        }

        if (vb.currentEvidence == true)
        {
            SendEvidenceEvent sendEvidenceEvent = new SendEvidenceEvent();
            sendEvidenceEvent.Evidence = (int)myEvidence;
            sendEvidenceEvent.positionOfTarget = transform.position;
            sendEvidenceEvent.gc = gc;

            switch (myEvidence)
            {
                case Evidence.None:
                    break;
                case Evidence.Picture:
                    sendEvidenceEvent.photoIndex = photoIndex;
                    break;
                case Evidence.MotionSensor:
                    sendEvidenceEvent.MotionSensorEvidence = ms;
                    break;
                case Evidence.SmokeGrenade:
                    sendEvidenceEvent.smokeGrenadeEvidence = sg;
                    break;
                case Evidence.PulseChecker:
                    sendEvidenceEvent.pulseCheckerEvidence = pc;
                    break;
            }

            EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.SHOW_EVIDENCE, sendEvidenceEvent);
        }
    }

    public void SetEvidence2(EventCallbacks.Event eventInfo)
    {
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        if (see.byteArray != null)
        {
            ba = see.byteArray;
            vb.currentEvidence = false;
        }
    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Setup || pc.previous == GamePhase.EndOfMeeting)
        {
            if (newEvidence != null)
                newEvidence.SetActive(false);
            myEvidence = Evidence.None;
            ba = null;
        }
    }
}
