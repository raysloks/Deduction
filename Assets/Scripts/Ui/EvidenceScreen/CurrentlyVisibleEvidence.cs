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
    }

    public void ShowEvidence(EventCallbacks.Event eventInfo)
    {
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;
        if (see.Evidence == 1)
        {
            Texture2D sampleTexture = new Texture2D(2, 2);

            bool isLoaded = sampleTexture.LoadImage(see.byteArray);
       //     sampleTexture = Resize(sampleTexture, sampleTexture.width * 8, sampleTexture.height * 8);

            ri.texture = sampleTexture;
        }

    }

    Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY, TextureFormat.RGB24, false);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;
    }

}
