using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigameInitiator : Interactable
{
    public GameObject minigame;

    public int minigame_index;

    public SpriteRenderer outline;

    public bool alwaysActive = false;

    private MinigamePopupScript popup;
    private GameController game;

    private void Awake()
    {
        popup = FindObjectOfType<MinigamePopupScript>();
        FindObjectOfType<TaskManager>().minigameInitiators.Add(minigame_index, this);
    }

    public override bool CanInteract(GameController game)
    {
        return alwaysActive || game.taskManager.tasks.Find(x => x.minigame_index == minigame_index && !x.completed) != null && game.player.role == 0 || 
            game.taskManager.sabotageTasks.Find(x => x.minigame_index == minigame_index) != null;
    }

    public override void Interact(GameController game)
    {
        this.game = game;
        popup.ActivatePopup(minigame, this);
    }

    public override void Highlight(bool highlighted)
    {
        if (outline != null)
        {
            if (highlighted)
                StartCoroutine(FadeInOutline(0.2f));
            else
                StartCoroutine(FadeOutOutline(0.2f));
        }
    }

    public void Solved()
    {
        int task_index = game.taskManager.tasks.FindIndex(x => x.minigame_index == minigame_index);
        if (task_index >= 0)
        {
            game.taskManager.tasks[task_index].completed = true;
            game.handler.link.Send(new TaskUpdate { task = (ushort)task_index });
        }
        else
        {
            int sabotage_index = game.taskManager.sabotageTasks.Find(x => x.minigame_index == minigame_index).sabotage;
            game.handler.link.Send(new SabotageTaskUpdate { sabotage = (ushort)sabotage_index });
        }
    }

    IEnumerator FadeInOutline(float seconds)
    {
        float counter = 0;

        while (counter < seconds)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOutOutline(float seconds)
    {
        float counter = seconds;

        while (counter > 0f)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter -= Time.deltaTime;
            yield return null;
        }
    }
}
