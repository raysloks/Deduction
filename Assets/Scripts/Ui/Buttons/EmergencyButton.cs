using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;

public class EmergencyButton : MonoBehaviour
{
    private bool ButtonActive = true;
    public Material outline;
    private Material m;
    private TextMeshPro text;
    private float maxTimer = 5f;
    private float timer = 0;
    private GameObject player = null;
    private List<GameObject> goPlayers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<SpriteRenderer>().material;
        text = transform.GetChild(0).transform.gameObject.GetComponent<TextMeshPro>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.RESET_TIMER, ButtonTimer);
        EventSystem.Current.RegisterListener(EVENT_TYPE.SETTINGS, Settings);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (ButtonActive == true && col.gameObject.GetComponent<Player>().emergencyButtonLeft == true)
            {
                Debug.Log("PlayerEntered1");

                //this.GetComponent<SpriteRenderer>().material = outline;
                //outline.SetFloat("_OutlineAlpha", 1f);
                text.text = "Vote";

                text.color = Color.green;
                col.gameObject.GetComponent<Player>().nearEmergencyButton = true;
            }
            else
            {
                if (ButtonActive == true)
                {
                    text.text = "X";
                }
                player = col.gameObject;
            }
            //ButtonActive = true;
            //buttonTimer();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (ButtonActive == true)
            {
                text.text = "Vote";
            }
            Debug.Log("PlayerExited");
            GetComponent<SpriteRenderer>().material = m;

            //outline.SetFloat("_OutlineAlpha", 0f);
            text.color = Color.black;
            col.gameObject.GetComponent<Player>().nearEmergencyButton = false;
            player = null;
            
            //ButtonActive = false;
        }
    }

    void ButtonTimer(EventCallbacks.Event eventinfo)
    {
        ButtonActive = false;
        StartCoroutine(WaitFunction2());
        text.text = "Vote";
    }

    IEnumerator WaitFunction2()
    {
        //const float waitTime = 3f;
        float counter = maxTimer;

        while (counter > 0)
        {
            text.text = Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null; //Don't freeze Unity
        }

        text.text = "Vote";
        ButtonActive = true;

        if (player != null)
        {
            if (player.GetComponent<Player>().emergencyButtonLeft == false)
            {
                //ButtonActive = false;
            }
            else
            {
                text.color = Color.green;
                player.GetComponent<Player>().nearEmergencyButton = true;
            }
        }
    }

    void Settings(EventCallbacks.Event eventInfo)
    {
        SettingEvent settingEvent = (SettingEvent)eventInfo;
        maxTimer = settingEvent.settings.emergencyMeetingCooldown / 1000000000;
    }
}
