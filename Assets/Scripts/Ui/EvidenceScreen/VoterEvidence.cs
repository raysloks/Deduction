using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class VoterEvidence : MonoBehaviour
{
    private RawImage ri;
    [HideInInspector] public enum Evidence { None, Picture };
    [HideInInspector] public Evidence myEvidence;
    [HideInInspector] public byte[] ba;
    List<Vector3> orignialPos = new List<Vector3>();
    private GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        myEvidence = Evidence.None;
        ri = GetComponent<RawImage>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.SEND_EVIDENCE2, SetEvidence2);

    }


    public void SetEvidence(EventCallbacks.Event eventInfo)
    {

        Debug.Log("SetEvidence1");
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        myEvidence = (Evidence)see.Evidence;
        if (myEvidence == Evidence.Picture)
        {
            Vector3 playerPos = see.positionOfTarget;           
            ScreenshotHandler.StartCameraFlash(0f, true, playerPos);
           
        }
    }
    public void SetEvidence2(EventCallbacks.Event eventInfo)
    {
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        Debug.Log("SetEvidence2 " +  see.byteArray.Length);
        ba = see.byteArray;
        
    }
}
