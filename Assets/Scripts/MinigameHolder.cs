using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHolder : MonoBehaviour
{
    public GameObject noteLocation;
    public GameController gameController;
    public GameObject stayCloseToTarget;

    // Start is called before the first frame update
    void Start()
    {
        if(noteLocation == null)
        {
            noteLocation = GameObject.Find("NoteLocations");
        }
        if (gameController == null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
        if(stayCloseToTarget == null)
        {
            stayCloseToTarget = GameObject.Find("StayCloseToTarget");
        }
    }

    
}
