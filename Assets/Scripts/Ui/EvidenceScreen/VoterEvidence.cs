using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class VoterEvidence : MonoBehaviour
{
    [HideInInspector] public enum Evidence { None, Picture, MotionSensor };
    [HideInInspector] public Evidence myEvidence;
    [HideInInspector] public byte[] ba;
    [HideInInspector] public  MotionSensor ms;
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
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        myEvidence = (Evidence)see.Evidence;
        Debug.Log((int)myEvidence);
        if (myEvidence == Evidence.Picture)
        {
            Debug.Log("PictureEvidence");
            
            // Vector3 playerPos = see.positionOfTarget;
            //ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height, true, playerPos, this);
            // ScreenshotHandler.StartCameraFlash(0f, true, playerPos);
        }else if(myEvidence == Evidence.MotionSensor)
        {
            Debug.Log("MotionEvidence");
            ms = see.MotionSensorEvidence;
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
