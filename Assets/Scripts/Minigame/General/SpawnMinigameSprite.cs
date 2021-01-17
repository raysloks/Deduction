using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinigameSprite : MonoBehaviour
{
    public GameObject minigame;
    private GameObject minigameObject;
    // Start is called before the first frame update
    void Start()
    {

        minigameObject = Instantiate(minigame);
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy1");
        Destroy(minigameObject);
    }
}
