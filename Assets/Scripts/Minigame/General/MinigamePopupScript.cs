using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventCallbacks;

public class MinigamePopupScript : MonoBehaviour
{
    public Player player;
    public GameObject minigamePrefab;
    public RectTransform minigameContainer;

    public List<AudioClip> victorySounds;

    public bool Active => minigame != null;

    private GameObject minigame;
    private MinigameInitiator initiator;

    public void ActivatePopup(GameObject prefab, MinigameInitiator ini)
    {
        if (minigame == null)
        {
            minigame = Instantiate(prefab, minigameContainer);
            if(ini != null)
            {
                initiator = ini;
            }
            player.canMove = false;
        }
    }

    public void DeactivatePopup()
    {
        if (minigame != null)
        {
            Destroy(minigame);
            minigame = null;
            initiator = null;
            player.canMove = true;
        }   
    }

    public void MinigameWon()
    {
        initiator.Solved();
        SoundEvent se = new SoundEvent();
        se.EventDescription = "Minigame got completed";
        se.UnitSound = victorySounds;
        se.UnitGameObjectPos = player.transform.position;
        EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
        StartCoroutine(EndIn(2));
    }

    IEnumerator EndIn(float sec)
    {
        float counter = sec;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            yield return null;
        }
        DeactivatePopup();
    }
}
