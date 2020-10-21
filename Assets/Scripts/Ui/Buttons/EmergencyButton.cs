using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;

public class EmergencyButton : MonoBehaviour
{
    public Material outline;
    private Material m;
    private TextMeshPro text;
    private Player player = null;
    private bool coolingDown = false;

    private GameController game;

    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<SpriteRenderer>().material;
        text = transform.GetChild(0).transform.gameObject.GetComponent<TextMeshPro>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, (EventCallbacks.Event ev) => PhaseChanged((PhaseChangedEvent)ev));
        game = FindObjectOfType<GameController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player = col.GetComponent<Player>();
            UpdateState();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.canRequestMeeting = false;
            player = null;
            UpdateState();
        }
    }

    void PhaseChanged(PhaseChangedEvent ev)
    {
        // start cooldown process
        if (ev.phase == GamePhase.Main)
            StartCoroutine(WaitFunction());
    }

    IEnumerator WaitFunction()
    {
        float counter = game.settings.emergencyMeetingCooldown / 1000000000f;

        coolingDown = true;

        while (counter > 0)
        {
            text.text = Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null; //Don't freeze Unity
        }

        text.text = "Vote";
        coolingDown = false;

        UpdateState();
    }

    private void UpdateState()
    {
        bool active = player != null && player.emergencyButtonsLeft > 0 && coolingDown == false;
        if (player != null)
            player.canRequestMeeting = active;
        text.color = active ? Color.green : Color.black;
    }
}
