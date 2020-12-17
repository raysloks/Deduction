using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMinigame : MonoBehaviour
{
    private GameController game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void Click()
    {
        game.popup.DeactivatePopup();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
