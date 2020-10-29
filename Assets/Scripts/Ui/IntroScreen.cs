using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EventCallbacks;

public class IntroScreen : MonoBehaviour
{
    public Text text;
    public Image sheen;
    public Transform mobDisplayContainer;
    public GameObject mobDisplayPrefab;
    public GameController game;
    public GameObject splash;

    private void Awake()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, x => PhaseChanged((PhaseChangedEvent)x));

        gameObject.SetActive(false);
    }

    private IEnumerator Display()
    {
        yield return new WaitForSeconds(3.0f);

        splash.SetActive(false);
        ulong role = game.player.role;
        sheen.color = role == 1 ? Color.red : Color.cyan;
        text.color = role == 1 ? Color.red : Color.white;
        text.text = role == 1 ? "Impostor" : "Crewmate";
        foreach (var n in game.handler.mobs)
        {
            Mob mob = n.Value;
            if (mob.role == role)
            {
                var go = Instantiate(mobDisplayPrefab, mobDisplayContainer);
                var image = go.GetComponent<Image>();
                image.sprite = mob.sprites[0];
                image.color = mob.sprite.color;
                var text = go.GetComponentInChildren<Text>();
                text.text = mob.name;
                text.color = role == 1 ? Color.red : Color.white;
            }
        }
    }

    private void PhaseChanged(PhaseChangedEvent phaseChangedEvent)
    {
        foreach (Transform child in mobDisplayContainer)
            Destroy(child.gameObject);
        if (phaseChangedEvent.phase == GamePhase.Intro)
        {
            gameObject.SetActive(true);
            splash.SetActive(true);
            StartCoroutine(Display());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
