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
    public Image blackout;

    private void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, x => PhaseChanged((PhaseChangedEvent)x));

        gameObject.SetActive(false);
    }

    private IEnumerator Display()
    {
        float counter = 0f;

        while (counter < 3.0f)
        {
            blackout.color = new Color(0f, 0f, 0f, Mathf.Max(1f - counter, counter - 2f));
            counter += Time.deltaTime;
            yield return null;
        }

        splash.SetActive(false);
        ulong role = game.player.role;
        sheen.color = role == 1 ? Color.red : Color.cyan;
        text.color = role == 1 ? Color.red : Color.white;
        text.text = role == 1 ? "Spy" : "Soldier";
        foreach (var n in game.handler.mobs)
        {
            Mob mob = n.Value;
            if (mob.role == role && game.handler.names.ContainsKey(n.Key))
            {
                var go = Instantiate(mobDisplayPrefab, mobDisplayContainer);
                var image = go.GetComponent<Image>();
                image.sprite = mob.sprite.sprite;
                image.color = mob.sprite.color;
                var text = go.GetComponentInChildren<Text>();
                text.text = mob.name;
                text.color = role == 1 ? Color.red : Color.white;
            }
        }

        while (counter < 6.0f)
        {
            blackout.color = new Color(0f, 0f, 0f, Mathf.Max(4f - counter, counter - 5f));
            counter += Time.deltaTime;
            yield return null;
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
