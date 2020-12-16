using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SabotageButtonManager : MonoBehaviour
{
    public Button sabotageButton;
    public GameObject[] sabotageMenus;
    public Text[] cooldownText;

    [HideInInspector] public int map;
   
    private GameController game;
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        game = FindObjectOfType<GameController>();
        sabotageButton.onClick.AddListener(() => sabotageMenus[map].SetActive(!sabotageMenus[map].activeSelf));
    }

    private void Update()
    {
        if (game.phase != GamePhase.Main || game.player.role != 1)
            sabotageMenus[map].SetActive(false);
        sabotageButton.gameObject.SetActive(game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText[map].gameObject.SetActive(game.time < game.player.sabotageCooldown && game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText[map].text = ((game.player.sabotageCooldown - game.time + 999999999) / 1000000000).ToString();

        if (game.phase == GamePhase.Main && game.player.role == 1)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                sabotageMenus[map].SetActive(true);
            if (Input.GetKeyUp(KeyCode.Tab))
                sabotageMenus[map].SetActive(false);
        }

        for (int i = 0; i < buttons.Count; ++i)
        {
            buttons[i].interactable = !game.taskManager.sabotageTasks.Any(x => x.sabotage == i);
        }
    }

    public void UpdateButtons()
    {
        buttons.Clear();
        foreach (Transform child in sabotageMenus[map].transform)
        {
            Button button = child.GetComponent<Button>();
            if (button)
            {
                int index = buttons.Count;
                buttons.Add(button);
                button.onClick.AddListener(() => game.Sabotage(index));
            }
        }
    }
}
