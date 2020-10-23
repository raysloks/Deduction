using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigameInitiator : MonoBehaviour
{
    public bool isSolved;
    public GameObject minigame;
    public MinigamePopupScript popup;
    
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

    public void StartMinigame()
    {
        popup.ActivatePopup(minigame, this.gameObject);
    }

    public void Solved()
    {
        isSolved = true;
    }
}
