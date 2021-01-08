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
    private List<List<string>> SensorList = new List<List<string>>();
    private List<List<int>> SensorList2 = new List<List<int>>();
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
            Debug.Log("ya dead son");
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
        int index = 0;
        foreach (var s in SensorList)
        {
            GameObject go = Instantiate(sensorPrefab, content);
            MotionSensor m = new MotionSensor();
            m.names = s;
            m.secondsIn = SensorList2[index];
            go.GetComponent<MotionSensorEvidence>().ms = m;
            index++;
        }
        
        gameObject.SetActive(false);
    }

    public void AddSensorList(List<string> sList, List<int> tList, int totalTime, List<Sprite> sprites, int number, List<ulong> playerIds)
    {
        Debug.Log("EvidenceHandler motion list");
        if (player.IsAlive == false)
        {
            Debug.Log("ya dead son");
            return;
        }
        Debug.Log("EvidenceHandler motion list2");
        GameObject go = Instantiate(sensorPrefab, content);
        MotionSensor m = new MotionSensor();
        m.names = sList;
        m.secondsIn = tList;
        m.totalRoundTime = totalTime;
        m.playerSprites = sprites;
        m.number = number;
        m.playerIds = playerIds;

        go.GetComponent<MotionSensorEvidence>().ms = m;
        go.GetComponent<MotionSensorEvidence>().SetText(number);
        Debug.Log("AddList");
        SensorList.Add(sList);
        SensorList2.Add(tList);
    }

    public void AddPulseCheckerEvidence(PulseCheckerEvidence pc)
    {
        GameObject go = Instantiate(pulsePrefab, content);
        go.GetComponent<PulseCheckerEvidencePrefab>().pce = pc;
    }

    public void AddSmokeGrenadeEvidence(SGEvidence sge)
    {
        //Only latest smoke is shown
        if(lastSmokePrefab != null)
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
            SensorList2.Clear();
            SensorList.Clear();
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
