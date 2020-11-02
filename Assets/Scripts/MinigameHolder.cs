using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHolder : MonoBehaviour
{
    public GameObject noteLocation;

    // Start is called before the first frame update
    void Start()
    {
        if(noteLocation == null)
        {
            noteLocation = GameObject.Find("NoteLocations");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
