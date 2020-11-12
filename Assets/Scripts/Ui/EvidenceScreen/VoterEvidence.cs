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
    // Start is called before the first frame update
    void Start()
    {
        myEvidence = Evidence.None;
        ri = GetComponent<RawImage>();
    }


    public void SetEvidence(EventCallbacks.Event eventInfo)
    {
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        myEvidence = (Evidence)see.Evidence;
        if (myEvidence == Evidence.Picture)
        {
            ba = see.byteArray;
            Texture2D sampleTexture = new Texture2D(2, 2);
            bool isLoaded = sampleTexture.LoadImage(see.byteArray);
            ri.texture = sampleTexture;
        }
    }
}
