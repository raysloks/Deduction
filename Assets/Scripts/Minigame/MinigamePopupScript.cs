using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamePopupScript : MonoBehaviour
{
    public Player player;
    public GameObject minigamePrefab;
    public RectTransform minigameContainer;

    private GameObject minigame;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePopup(string sceneName, GameObject ini)
    {
        if (minigame == null)
            minigame = Instantiate(minigamePrefab, minigameContainer);
    }

    public void DeactivatePopup(bool complete)
    {
        if (minigame != null)
        {
            Destroy(minigame);
            minigame = null;
        }   
    }

    public void MinigameWon()
    {
    }
}
