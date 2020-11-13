using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class CurrentlyVisibleEvidence : MonoBehaviour
{
    public GameObject vis;
    private RawImage ri;
    // Start is called before the first frame update
    void Start()
    {
        ri = GetComponent<RawImage>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.SHOW_EVIDENCE, ShowEvidence);

        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }

    public void ShowEvidence(EventCallbacks.Event eventInfo)
    {

        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;

        if (see.Evidence == 1)
        {
            Debug.Log("Show Evidence byteLenght " + see.byteArray.Length);
            Texture2D sampleTexture = new Texture2D(2, 2);

            bool isLoaded = sampleTexture.LoadImage(see.byteArray);

            ri.texture = sampleTexture;
            
        }

    }
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Main)
        {
            ri.texture = null;
        }
    }


}
