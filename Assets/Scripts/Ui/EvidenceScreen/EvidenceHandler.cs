using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;
using System.Linq;

public class EvidenceHandler : MonoBehaviour
{
    public Transform content;
    public GameObject picturePrefab;
    public GameObject sensorPrefab;
    public GameObject pulsePrefab;
    public GameObject smokePrefab;
    public Player player;
    private GameObject lastSmokePrefab;

    private void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }

    public void AddAllEvidence()
    {
        if(player.IsAlive == false)
        {
            this.gameObject.SetActive(false);
            return;
        }
        GameController game = FindObjectOfType<GameController>();
        foreach (var n in game.screenshotHandler.photos)
        {
            Photo photo = n.Value;
            if (photo.poses[photo.photographer].index == game.handler.playerMobId)
            {
                var go = Instantiate(picturePrefab, content);
                go.GetComponent<RawImage>().texture = photo.texture;
                go.GetComponent<EvidencePicture>().photoIndex = (int)n.Key;
            }
        }
        
        gameObject.SetActive(false);
    }

    public void AddSensorList(MotionSensor m)
    {
        if (player.IsAlive == false)
        {
            this.gameObject.SetActive(false);
            return;
        }
        GameObject go = Instantiate(sensorPrefab, content);
        

        go.GetComponent<MotionSensorEvidence>().ms = m;
        go.GetComponent<MotionSensorEvidence>().SetText(m.number);
    }

    public void AddPulseCheckerEvidence(PulseCheckerEvidence pc)
    {
        if (player.IsAlive == false)
        {
            this.gameObject.SetActive(false);
            return;
        }
        GameObject go = Instantiate(pulsePrefab, content);
        go.GetComponent<PulseCheckerEvidencePrefab>().pce = pc;
    }

    public void AddSmokeGrenadeEvidence(SGEvidence sge)
    {
        if (player.IsAlive == false)
        {
            this.gameObject.SetActive(false);
            return;
        }
        //Only latest smoke is shown
        if (lastSmokePrefab != null)
        {
            Destroy(lastSmokePrefab);
        }
        lastSmokePrefab = Instantiate(smokePrefab, content);
        lastSmokePrefab.GetComponent<SmokeGrenadeEvidencePrefab>().sge = sge;

    }

    //End of meeting cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.previous == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            gameObject.SetActive(true);
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
