using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EventCallbacks;

public class GameOverScreen : MonoBehaviour
{
    public Text text;
    public Image sheen;
    public Transform winnerContainer;
    public GameObject winnerPrefab;

    private void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.GAME_OVER, x => Activate((GameOverEvent)x));
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, x => PhaseChanged((PhaseChangedEvent)x));

        gameObject.SetActive(false);
    }

    private void Activate(GameOverEvent gameOverEvent)
    {
        gameObject.SetActive(true);
        text.text = gameOverEvent.victory ? "Victory" : "Defeat";
        text.color = gameOverEvent.victory ? Color.cyan : Color.red;
        sheen.color = gameOverEvent.role == 0 ? Color.cyan : Color.red;
        foreach (Mob mob in gameOverEvent.winners)
        {
            var go = Instantiate(winnerPrefab, winnerContainer);
            var image = go.GetComponent<Image>();
            image.sprite = mob.sprite.sprite;
            image.color = mob.sprite.color;
            var text = go.GetComponentInChildren<Text>();
            text.text = mob.name;
        }
    }

    private void PhaseChanged(PhaseChangedEvent phaseChangedEvent)
    {
        if (phaseChangedEvent.phase != GamePhase.GameOver)
        {
            gameObject.SetActive(false);
            foreach (Transform child in winnerContainer)
                Destroy(child.gameObject);
        }
    }
}
