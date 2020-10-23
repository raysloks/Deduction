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
    private MinigameInitiator initiator;

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

    public void ActivatePopup(GameObject prefab, GameObject ini)
    {
        if (minigame == null)
        {
            minigame = Instantiate(prefab, minigameContainer);
            initiator = ini.GetComponent<MinigameInitiator>();
        }
    }

    public void DeactivatePopup()
    {
        if (minigame != null)
        {
            Destroy(minigame);
            minigame = null;
            initiator = null;
        }   
    }

    public void MinigameWon()
    {
        initiator.Solved();
    }
}
