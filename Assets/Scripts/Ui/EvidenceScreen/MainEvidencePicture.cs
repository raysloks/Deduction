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
    private Image buttonImage;
    private bool sentEvidence = false;
    [HideInInspector]public bool lockable = false;
    public Texture texture;
    public Button lockButton;
    private UnityEngine.Color normalColor;
    private UnityEngine.Color pressedColor;
    // Start is called before the first frame update
    void Start()
    {
        ri = MainPicture.GetComponent<RawImage>();
        buttonImage = lockButton.GetComponent<Image>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
        var colors = lockButton.colors;
        normalColor = colors.normalColor;
        pressedColor = colors.pressedColor;
    }

    public void SetMainPicture(Texture tex)
    {
        ri.texture = tex;
    }

    public void LockEvidencePicture(List<Vector3> pos, Vector3 player)
    {
        if (sentEvidence == false)
        {
            lockButton.interactable = false;
            lockable = false;
            sentEvidence = true;
            gc.SendEvidence(pos, player);
        }
    }
    public void ClickLock()
    {
        lockable = !lockable;
        changeButtonColor(lockable);
    }
    private void changeButtonColor(bool l)
    {

        if (l == true)
            buttonImage.color = pressedColor;
        else
            buttonImage.color = normalColor;

    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.previous == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            lockable = false;
            changeButtonColor(lockable);
            sentEvidence = false;
            ri.texture = null;
            lockButton.interactable = true;
        }
    }

}
