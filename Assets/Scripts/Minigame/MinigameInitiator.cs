using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigameInitiator : Interactable
{
    public GameObject minigame;

    public int minigame_index;

    private MinigamePopupScript popup;
    private GameController game;

    private void Awake()
    {
        popup = FindObjectOfType<MinigamePopupScript>();
    }

    public override bool CanInteract(GameController game)
    {
        return game.taskManager.tasks.Find(x => x.minigame_index == minigame_index) != null;
    }

    public override void Interact(GameController game)
    {
        this.game = game;
        popup.ActivatePopup(minigame, this);
    }

    public void Solved()
    {
        int task_index = game.taskManager.tasks.FindIndex(x => x.minigame_index == minigame_index);
        game.taskManager.tasks[task_index].completed = true;
        game.handler.link.Send(new TaskUpdate { task = (ushort)task_index });
    }
}
