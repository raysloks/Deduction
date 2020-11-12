using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SabotageButtonManager : MonoBehaviour
{
    public GameObject sabotageButtonContainer;
    public List<Button> buttons;

    public Text cooldownText;

    private GameController game;

    private void Awake()
    {
        game = FindObjectOfType<GameController>();
        buttons.Clear();
        foreach (Transform child in sabotageButtonContainer.transform)
        {
            Button button = child.GetComponent<Button>();
            if (button)
                buttons.Add(button);
        }
    }

    private void Update()
    {
        sabotageButtonContainer.SetActive(game.time > game.player.sabotageCooldown && game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText.gameObject.SetActive(game.time < game.player.sabotageCooldown && game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText.text = ((game.player.sabotageCooldown - game.time) / 1000000000).ToString();

        for (int i = 0; i < buttons.Count; ++i)
        {
            buttons[i].interactable = !game.taskManager.sabotageTasks.Any(x => x.sabotage == i);
        }
    }
}
