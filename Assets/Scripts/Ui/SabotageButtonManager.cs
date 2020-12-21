using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SabotageButtonManager : MonoBehaviour
{
    public Button sabotageButton;
    public GameObject[] sabotageMenus;
    public Image[] playerIcons;
    public Text[] cooldownTexts;

    [HideInInspector] public int map;
   
    private GameController game;
    private List<Button> buttons = new List<Button>();
    private Sprite playerSprite;
    private Vector2[] mapOffsets = {new Vector2(0, 0), new Vector2(-581 + 750, 314 - 150 + 10) };
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
        cooldownTexts[map].gameObject.SetActive(game.time < game.player.sabotageCooldown && game.phase == GamePhase.Main && game.player.role == 1);
        cooldownTexts[map].text = ((game.player.sabotageCooldown - game.time + 999999999) / 1000000000).ToString();

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

        playerIcons[map].transform.localPosition = ((Vector2)game.player.transform.position * 16) + mapOffsets[map];
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
        StartCoroutine(GetPlayerSprite());
    }

    IEnumerator GetPlayerSprite()
    {
        yield return new WaitForSeconds(0.5f);
        playerSprite = game.player.sprite.sprite;
        playerIcons[map].sprite = playerSprite;
        playerIcons[map].color = game.player.characterTransform.GetComponentInChildren<SpriteRenderer>().color;
    }
}
