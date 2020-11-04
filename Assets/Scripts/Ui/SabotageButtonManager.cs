using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SabotageButtonManager : MonoBehaviour
{
    public GameObject sabotageButtonContainer;
    public List<Button> buttons;

    public Text cooldownText;

    private GameController game;

    private void Awake()
    {
        game = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        sabotageButtonContainer.SetActive(game.time > game.player.sabotageCooldown && game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText.gameObject.SetActive(game.time < game.player.sabotageCooldown && game.phase == GamePhase.Main && game.player.role == 1);
        cooldownText.text = ((game.player.sabotageCooldown - game.time) / 1000000000).ToString();
    }
}
