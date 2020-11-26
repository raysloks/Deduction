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
    private List<List<string>> SensorList = new List<List<string>>();

    private void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }

    public void AddAllEvidence()
    {
        GameController game = FindObjectOfType<GameController>();
        foreach (var n in game.screenshotHandler.photos)
        {
            Photo photo = n.Value;
            if (photo.poses[photo.photographer].index == game.handler.playerMobId)
            {
                var go = Instantiate(picturePrefab, content);
                go.GetComponent<RawImage>().texture = photo.texture;
            }
        }

        foreach (var s in SensorList)
        {
            GameObject go = Instantiate(sensorPrefab, content);
            MotionSensor m = new MotionSensor();
            m.names = s;
            go.GetComponent<MotionSensorEvidence>().ms = m;
           // go.AddList(s);
        }
        gameObject.SetActive(false);
    }

    public void AddSensorList(List<string> sList)
    {
        SensorList.Add(sList);
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
