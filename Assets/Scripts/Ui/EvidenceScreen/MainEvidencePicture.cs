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
using System.Linq;

public class MainEvidencePicture : MonoBehaviour
{
    [HideInInspector] public bool lockable = false;
    [HideInInspector] public bool PictureShowing = false;
    public GameObject MainPicture;
    public GameController gc;
    public Texture texture;
    public Button lockButton;
    public GameObject motionSensorMain;
    public GameObject pulseCheckerMain;
    public GameObject smokeGrenadeMain;

    private RawImage ri;
    private Image buttonImage;
    private bool sentEvidence = false;
    private UnityEngine.Color normalColor; //Can't build without "UnityEngine."
    private UnityEngine.Color pressedColor;

    private void Start()
    {
        ri = MainPicture.GetComponent<RawImage>();
        buttonImage = lockButton.GetComponent<Image>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
        var colors = lockButton.colors;
        normalColor = colors.normalColor;
        pressedColor = colors.pressedColor;
    }

    //Quick method to change the main pictures image
    public void SetMainPicture(Texture tex)
    {
        PictureShowing = true;
        pulseCheckerMain.SetActive(false);
        motionSensorMain.SetActive(false);
        smokeGrenadeMain.SetActive(false);
        ri.enabled = true;
        ri.texture = tex;
    }

    public void SetMainSensorList(MotionSensor ms)
    {
        PictureShowing = false;
        ri.enabled = false;
        smokeGrenadeMain.SetActive(false);
        pulseCheckerMain.SetActive(false);
        motionSensorMain.SetActive(true);
        motionSensorMain.GetComponent<ShowMotionSensorList>().addAllOptions(ms);
    }

    public void SetPulseChecker(PulseCheckerEvidence pc)
    {
        PictureShowing = false;
        ri.enabled = false;
        motionSensorMain.SetActive(false);
        smokeGrenadeMain.SetActive(false);
        pulseCheckerMain.SetActive(true);
        pulseCheckerMain.GetComponent<ShowPulseEvidence>().DisplayPulseEvidence(pc);
    }

    public void SetSmokeGrenade(SGEvidence sge)
    {
        PictureShowing = false;
        pulseCheckerMain.SetActive(false);
        motionSensorMain.SetActive(false);
        ri.enabled = false;
        smokeGrenadeMain.SetActive(true);
        smokeGrenadeMain.GetComponent<ShowSmokeGrenade>().DisplaySmokeEvidence(sge);
    }

    //When you choose a picture send it to the GC that will send a Send Evidence message to the handler.
    public void LockEvidencePicture(int photoIndex)
    {
        if (sentEvidence == false)
        {
            lockButton.interactable = false;
            lockable = false;
            sentEvidence = true;
            gc.handler.link.Send(new PresentEvidence { index = (ulong)photoIndex, sensornames = new List<byte>() });
        }
    }

    //When you choose a sensorlist send it to the GC that will send a Send Evidence message to the handler.
    public void LockEvidenceSensor(MotionSensor ms)
    {
        if (sentEvidence == false)
        {
            lockButton.interactable = false;
            lockable = false;
            sentEvidence = true;
            string s = "";
            int index = 0;
            if(ms.names.Count > 0)
            {
                foreach (string s2 in ms.names)
                {
                    if (index == (ms.names.Count - 1))
                        s += s2;
                    else
                        s += s2 + ";";
                    index++;
                }
            }
           

            List<byte> dataAsBytes = Encoding.ASCII.GetBytes(s).ToList();

            if (ms.names != null)
            {
                SendSensorList message = new SendSensorList
                {
                    names = dataAsBytes,
                    times = ms.secondsIn,
                    player = gc.handler.playerMobId,
                    playerIds = ms.playerIds,
                    totalRoundTime = ms.totalRoundTime
                };
                gc.handler.link.Send(message);
            }
        }
    }

    public void LockPulseChecker(PulseCheckerEvidence pc)
    {
        if (sentEvidence == false)
        {
            lockButton.interactable = false;
            lockable = false;
            sentEvidence = true;

            PulseEvidence message = new PulseEvidence
            {
                deadTime = pc.Time,
                playerId = pc.playerId,
                deadId = pc.deadId
            };
            gc.handler.link.Send(message);
        }
    }

    public void LockSmokeGrenade(SGEvidence sg)
    {
        if (sentEvidence == false)
        {
            lockButton.interactable = false;
            lockable = false;
            sentEvidence = true;

            SmokeGrenadeEvidence message = new SmokeGrenadeEvidence
            {
                area = sg.area,
                playerName = sg.playerName,
                playerId = sg.playerID
            };
            gc.handler.link.Send(message);
        }
    }

    //If you press the lock button
    public void ClickLock()
    {
        lockable = !lockable;
        ChangeButtonColor(lockable);
    }
	
    private void ChangeButtonColor(bool l)
    {
        buttonImage.color = l ? pressedColor : normalColor;
    }

    //End Of Meeting Cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.previous == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            lockable = false;
            ChangeButtonColor(lockable);
            sentEvidence = false;
            ri.texture = null;
            lockButton.interactable = true;

        }
    }

}
