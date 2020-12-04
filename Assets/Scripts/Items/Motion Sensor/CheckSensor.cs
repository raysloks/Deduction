using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class CheckSensor : MonoBehaviour
{
    private int enter = 0;
    private List<string> peopleEntered = new List<string>();
    private List<int> secondsIn = new List<int>();
    [HideInInspector] public GameController gc;
    [HideInInspector] public EvidenceHandler evidenceHandler;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
        EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_STARTED, meetingStarted);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Mob")
        {
            enter++;
            peopleEntered.Add(col.gameObject.name);
            int timer = (int)gc.roundTimer;
            secondsIn.Add(timer);
            Debug.Log("Enter Col: " + col.gameObject.name + " Number entered: " + enter+ " Seconds In round : " + timer);
        }
    }

    private void sendList()
    {
        SendSensorList message = new SendSensorList
        {
            names = peopleEntered
        };
        gc.handler.link.Send(message);
    }

    public void meetingStarted(EventCallbacks.Event eventinfo)
    {
        evidenceHandler.AddSensorList(peopleEntered, secondsIn);
    }


    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Discussion)
        {
         //   evidenceHandler.AddSensorList(peopleEntered);
        }

        if(pc.previous == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            Destroy(this.gameObject);
        }
    }
}
