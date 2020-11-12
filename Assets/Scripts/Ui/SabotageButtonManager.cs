using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SabotageButtonManager : MonoBehaviour
{
    public Button sabotageButton;
    public GameObject sabotageButtonContainer;

    public Text cooldownText;

    private GameController game;
    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        game = FindObjectOfType<GameController>();
        foreach (Transform child in sabotageButtonContainer.transform)
        {
            Button button = child.GetComponent<Button>();
            if (button)
            {
                int index = buttons.Count;
                buttons.Add(button);
                button.onClick.AddListener(() => game.Sabotage(index));
            }
        }
        sabotageButton.onClick.AddListener(() => sabotageButtonContainer.SetActive(!sabotageButtonContainer.activeSelf));
    }

    private void Update()
    {
        sabotageButton.gameObject.SetActive(game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText.gameObject.SetActive(game.time < game.player.sabotageCooldown && game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText.text = ((game.player.sabotageCooldown - game.time + 999999999) / 1000000000).ToString();

        for (int i = 0; i < buttons.Count; ++i)
        {
            buttons[i].interactable = !game.taskManager.sabotageTasks.Any(x => x.sabotage == i);
        }
    }
}
