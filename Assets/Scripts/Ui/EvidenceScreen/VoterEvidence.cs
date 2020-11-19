using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class VoterEvidence : MonoBehaviour
{
    [HideInInspector] public enum Evidence { None, Picture };
    [HideInInspector] public Evidence myEvidence;
    [HideInInspector] public byte[] ba;
    private GameController gc;
    public VoteButton vb;
    public GameObject newEvidence;

    // Start is called before the first frame update
    void Start()
    {
        myEvidence = Evidence.None;
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);

    }

    //Sent from Meeting UI listener. 
    public void SetEvidence(EventCallbacks.Event eventInfo)
    {
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        myEvidence = (Evidence)see.Evidence;
        if (myEvidence == Evidence.Picture)
        {
            Vector3 playerPos = see.positionOfTarget;
            ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height, true, playerPos, this);
        }
    }

    //End Of Meeting Cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Setup || pc.previous == GamePhase.EndOfMeeting)
        {
            newEvidence.SetActive(false);
            myEvidence = Evidence.None;
            ba = null;
        }
    }
}
