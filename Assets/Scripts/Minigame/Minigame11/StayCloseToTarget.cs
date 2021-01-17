using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventCallbacks;

public class StayCloseToTarget : MonoBehaviour
{
    //Script that checks if other players are close when Minigame 11 is active

    private Vector3 orignialScale;
    private bool reduce = false;
    private bool isDone = false;
    private Transform smallbar;
    private Player player;
    private int numberOfActive = 0;
    List<GameObject> mobs = new List<GameObject>();

    public float sliderGain = 0.1f; 
    public List<AudioClip> correctSounds;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, resetGame);
        smallbar = transform.GetChild(0).gameObject.transform.GetChild(2);
        orignialScale = smallbar.localScale;
        player = transform.parent.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reduce)
        {
            if (smallbar.localScale.x > 0.05f)
            {
                smallbar.localScale -= new Vector3(((sliderGain * 2) * Time.deltaTime), 0f, 0f);
            }
        }
        if(mobs.Count > 0 && !isDone)
        {
            if (player.canMove == true)
            {
                smallbar.localScale += new Vector3((sliderGain * Time.deltaTime), 0f, 0f);
                if (smallbar.localScale.x >= 2f)
                {
                    isDone = true;
                    SoundEvent se = new SoundEvent();
                    se.UnitSound = correctSounds;
                    se.UnitGameObjectPos = transform.position;
                    EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
                }
            }         
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Mob" && mobs.Contains(col.gameObject) == false)
        {
            reduce = false;
            mobs.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Mob" && mobs.Contains(col.gameObject) == true)
        {         
            mobs.Remove(col.gameObject);
            if (mobs.Count == 0 && isDone == false)
            {
                reduce = true;
            }
        }
    }

    public bool getIsDone()
    {
        return isDone;
    }

    public int getNumberOfActive()
    {
        return numberOfActive;
    }

    public void SetNumberOfActives(int set)
    {
        this.numberOfActive += set;
    }

    public void resetSlider()
    {
        smallbar.localScale = orignialScale;
        isDone = false;
    }

    public void resetGame(EventCallbacks.Event e)
    {
        PhaseChangedEvent phaseChangeEventInfo = (PhaseChangedEvent)e;
        if (phaseChangeEventInfo.phase == GamePhase.Setup)
        {           
            resetSlider();
            gameObject.SetActive(false);
        }
    }
}
