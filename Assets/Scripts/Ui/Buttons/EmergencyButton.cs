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

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
        game = FindObjectOfType<GameController>();
    }

    private void OnDestroy()
    {
        EventSystem.Current.UnregisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }

    private void PhaseChanged(EventCallbacks.Event ev)
    {
        PhaseChangedEvent phaseChangedEvent = (PhaseChangedEvent)ev;
        // start cooldown process
        if (phaseChangedEvent.phase == GamePhase.Main)
            StartCoroutine(WaitFunction());
    }

    private IEnumerator WaitFunction()
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
        return game.player.emergencyButtonsLeft > 0 && game.player.IsAlive && coolingDown == false && game.phase == GamePhase.Main;
    }

    public override void Interact(GameController game)
    {
        game.handler.link.Send(new MeetingRequested());
    }
}
