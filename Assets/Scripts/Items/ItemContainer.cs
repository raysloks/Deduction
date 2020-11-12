using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using EventCallbacks;
public class ItemContainer : MonoBehaviour
{

    [HideInInspector] public enum Item { None, Camera };
    [HideInInspector] public Item item;
    private bool waitingForCountDown = false;
    private bool coolingDown = false;
    public TextMeshPro text;
    public SpriteRenderer sr;
    public Sprite cameraSprite;
    // Start is called before the first frame update
    void Start()
    {
       // item = (Item)UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));
        Restock(UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1)));

        EventSystem.Current.RegisterListener(EVENT_TYPE.PICKUP_WAIT, waitForRestock);
    }


    public void ItemTaken()
    {
        item = Item.None;
        sr.sprite = null;

    }

    private IEnumerator WaitFunction()
    {
        float counter = 30f;

        coolingDown = true;

        while (counter > 1)
        {
            text.text = Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null; //Don't freeze Unity
        }

        text.text = "";
        coolingDown = false;
        Restock(UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1)));

    }

    public void waitForRestock(EventCallbacks.Event eventInfo)
    {
        CooldownEvent e = (CooldownEvent)eventInfo;

        int lookFor = e.child;
        Debug.Log("LookFor " + lookFor);
        int index = 0;
        Transform p = transform.parent;
        foreach (Transform child in p)
        {
            if(child == this.transform)
            {
                if (index == (lookFor - 1))
                {
                    StartCoroutine(WaitFunction());
                }
                break;
            }
            index++;
        }
    }

    private void Restock(int itemNr)
    {
        item = (Item)itemNr;
        switch (item)
        {
            case Item.Camera:
                sr.sprite = cameraSprite;
                break;
        }
    }

}
