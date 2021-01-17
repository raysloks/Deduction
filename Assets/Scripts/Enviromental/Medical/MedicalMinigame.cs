using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MedicalMinigame : MonoBehaviour
{
    public Transform[] mobDisplayContainers;
    public GameObject mobDisplayPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMobs(Mob[] mobs)
    {
        Debug.Log("Setting heads");

        for (int i = 0; i < 10; i++)
        {
            if (i < mobs.Length)
            {
                mobDisplayContainers[Mathf.FloorToInt(i / 5)].GetChild(i - (Mathf.FloorToInt(i / 5) * 5)).gameObject.SetActive(true);
                var image = mobDisplayContainers[Mathf.FloorToInt(i / 5)].GetChild(i).GetChild(0).GetComponent<Image>();
                image.sprite = mobs[i].sprite.sprite;
                image.color = mobs[i].sprite.color;
                mobDisplayContainers[0].GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = mobs[i].name;
                mobDisplayContainers[0].GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = mobs[i].IsAlive == true ? "<color=green>Alive" : "<color=red>Deceased";
                Debug.Log(mobs[i].IsAlive == true ? "Alive" : "Deceased");
            }
            else
                mobDisplayContainers[Mathf.FloorToInt(i / 5)].GetChild(i - (Mathf.FloorToInt(i/5) * 5)).gameObject.SetActive(false);
        }
        /*
        foreach (Mob mob in mobs)
        {
            Debug.Log("+1 head");
            var sopp = new GameObject();
            sopp.transform.parent = mobDisplayContainers[0].transform;
            sopp.name = mob.name + " - soppig";
            var go = Instantiate(mobDisplayPrefab, mobDisplayContainers[0]);
            var image = go.GetComponent<Image>();
            image.sprite = mob.sprite.sprite;
            image.color = mob.sprite.color;
            Debug.Log(mob.sprite.color);
            var text = go.GetComponentInChildren<Text>();
            text.text = mob.name;
            //text.color = role == 1 ? Color.red : Color.white;
        }*/
    }
}