using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SabotageButtonManager : MonoBehaviour
{
    public GameObject sabotageButtonContainer;
    public List<Button> buttons;

    private GameController game;

    private void Awake()
    {
        game = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        sabotageButtonContainer.SetActive(game.phase == GamePhase.Main && game.player.role == 1);
    }
}
