using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class HM : MonoBehaviour
{
    //Script for controlling pulse checkers heart monitor animation
    private Material m;
    private bool alive = true;
    private float timer = -1f;

    public Material OrgMaterial;
    public GameObject parent;
    public Animation animation;

    // Start is called before the first frame update
    void Start()
    {
        m = this.GetComponent<SpriteRenderer>().material;
        m = OrgMaterial;
        Debug.Log("Set to 1");
        m.SetFloat("_OffsetUvX", 1f);
        EventCallbacks.EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);


    }

    public void Activate(bool set)
    {
        parent.SetActive(set);
    }

    public void SetAlive(bool aliveOrDead)
    {
        this.alive = aliveOrDead;
        if(alive == false)
        {
            animation.Stop();
            animation.enabled = false;
          //  m.SetFloat("OffsetUvX", -1f);
        }
        else
        {

            animation.enabled = true;
            animation.Play();
        }
    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            Activate(false);
        }
    }
}
