using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamePopupScript : MonoBehaviour
{
    public GameObject player;
    public GameObject popup;
    
    private string scene;
    private GameObject initiator;
    

    private void Awake()
    {
        popup.GetComponent<MeshRenderer>().enabled = false;
        initiator = null;
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
        initiator = ini;
        scene = sceneName;
        //popup.transform.position = player.transform.position;
        //popup.transform.position = new Vector3(popup.transform.position.x, popup.transform.position.y, 2);
        //popup.GetComponent<MeshRenderer>().enabled = true;
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public void DeactivatePopup(bool complete)
    {
        popup.GetComponent<MeshRenderer>().enabled = false;
        SceneManager.UnloadScene(scene);
        scene = null;
        initiator = null;
    }

    public void MinigameWon()
    {
        initiator.GetComponent<MinigameInitiator>().Solved();
    }
}
