using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;

public class EmergencyButton : Interactable
{
    public Material outline;
    private TextMeshPro text;
    private bool coolingDown = false;

    private GameController game;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, (EventCallbacks.Event ev) => PhaseChanged((PhaseChangedEvent)ev));
        game = FindObjectOfType<GameController>();
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
    }

    public override bool CanInteract(GameController game)
    {
        return game.player.emergencyButtonsLeft > 0 && game.player.IsAlive && coolingDown == false;
    }

    public override void Interact(GameController game)
    {
        game.handler.link.Send(new MeetingRequested());
    }
}
