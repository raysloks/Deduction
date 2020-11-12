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
            Vector3 playerPos = Vector3.zero;
            ulong index = 0;
            gc = see.gc;
            foreach (Vector3 v in see.vec3List)
            {
                if(index == see.idOfTarget)
                {
                    Debug.Log("Player is " + index + see.idOfTarget);
                    playerPos = v;
                }
                orignialPos.Add(see.gc.handler.mobs[index].transform.position);
               // see.gc.handler.mobs[index].transform.position = v;
                index++;
            }
            ScreenshotHandler.StartCameraFlash(0f, true, playerPos);
           
        }
    }
    public void SetEvidence2(EventCallbacks.Event eventInfo)
    {
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        Debug.Log("SetEvidence2 " +  see.byteArray.Length);
        /*
        int index = 0;
        foreach (KeyValuePair<ulong, Mob> m in gc.handler.mobs)
        {
            m.Value.transform.position = orignialPos[index];
            index++;
        }
        */
        ba = see.byteArray;
        Texture2D sampleTexture = new Texture2D(2, 2);
        bool isLoaded = sampleTexture.LoadImage(see.byteArray);
        ri.texture = sampleTexture;
        
    }
}
