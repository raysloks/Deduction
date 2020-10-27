using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigameInitiator : MonoBehaviour
{
    public bool isSolved;
    public GameObject minigame;
    public MinigamePopupScript popup;

    // very temp
    private int index;
    private GameController game;
    
    //GameObject.Find("PopupWindow").GetComponent<MinigamePopupScript>();
    // Start is called before the first frame update
    void Start()
    {
        isSolved = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMinigame()
    {
        isSolved = false;
    }

    public void StartMinigame(int index, GameController game)
    {
        this.index = index;
        this.game = game;
        popup.ActivatePopup(minigame, this.gameObject);
    }

    public void Solved()
    {
        game.taskManager.tasks[index].completed = true;
        game.handler.link.Send(new TaskUpdate { task = (ushort)index });
        isSolved = true;
    }
}
