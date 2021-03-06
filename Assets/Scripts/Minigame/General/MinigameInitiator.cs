﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigameInitiator : Interactable
{
    public GameObject minigame;
    public GameObject altMinigame;
    public int minigame_index;
    public bool alwaysActive = false;

    private GameController game;
    private AudioSource audioSource;
    private bool accessibilityMode;
    private void Start()
    {
        if (minigame_index > 0)
            FindObjectOfType<TaskManager>().minigameInitiators[minigame_index] = this;
        game = FindObjectOfType<GameController>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        if (audioSource)
        {
            bool shouldPlaySound = alwaysActive || game.taskManager.tasks.Find(x => x.minigame_index == minigame_index && !x.completed) != null && game.player.role == 0 ||
               game.taskManager.sabotageTasks.Find(x => x.minigame_index == minigame_index) != null;
            if (shouldPlaySound && !audioSource.isPlaying)
                audioSource.Play();
            if (!shouldPlaySound && audioSource.isPlaying)
                audioSource.Stop();
        }
        
        if (accessibilityMode != game.GetComponent<GameSettingsManager>().accessibilityMode)
        {
            if (altMinigame != null)
            {
                var temp = minigame;
                minigame = altMinigame;
                altMinigame = temp;
            }
            accessibilityMode = !accessibilityMode;
        }
    }

    public override bool CanInteract(GameController game)
    {
        return alwaysActive || (game.taskManager.tasks.Find(x => x.minigame_index == minigame_index && !x.completed) != null && game.player.role == 0) || 
            (game.taskManager.sabotageTasks.Find(x => x.minigame_index == minigame_index) != null && game.player.IsAlive);
    }

    public override void Interact(GameController game)
    {
        game.popup.ActivatePopup(minigame, this);
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
}