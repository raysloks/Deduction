using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using UnityEngine.UI;
using System.Drawing;
using System.Text;
using System;
using EventCallbacks;

public class MainEvidencePicture : MonoBehaviour
{
    public GameObject MainPicture;
    public GameController gc;
    private RawImage ri;
    private bool sentEvidence = false;
    public Texture texture;
    // Start is called before the first frame update
    void Start()
    {
        ri = MainPicture.GetComponent<RawImage>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);

    }

    public void SetMainPicture(Texture tex)
    {
        ri.texture = tex;
    }

    public void LockEvidencePicture(List<Vector3> pos, Vector3 player)
    {
        if (sentEvidence == false)
        {
            sentEvidence = true;
            gc.SendEvidence(pos, player);
        }
    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Main || pc.phase == GamePhase.Setup)
        {
            sentEvidence = false;
            ri.texture = null;
        }
    }

}
