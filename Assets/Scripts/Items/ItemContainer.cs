using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using EventCallbacks;

public class ItemContainer : MonoBehaviour
{

    [HideInInspector] public enum Item { None, Camera, MotionSensor, SmokeGrenade };
    [HideInInspector] public Item item;
    private bool waitingForCountDown = false;
    private bool coolingDown = false;
    public TextMeshPro text;
    public SpriteRenderer sr;
    public Sprite cameraSprite;
    public Sprite motionSensorSprite;
    public Sprite smokeGrenadeSprite;

    // Start is called before the first frame update
    void Start()
    {
        Restock(UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1)));
        EventSystem.Current.RegisterListener(EVENT_TYPE.PICKUP_WAIT, waitForRestock);
    }


    public void ItemTaken()
    {
        item = Item.None;
        sr.sprite = null;
    }

    private IEnumerator WaitFunction(int random)
    {
        ItemTaken();
        float counter = 30f;

        coolingDown = true;

        while (counter > 1)
        {
            text.text = Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null;
        }

        text.text = "";
        coolingDown = false;
        if(random == 2)
        {
            random++;
        }
        Restock(random);

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
                if (index == (lookFor))
                {
                    StartCoroutine(WaitFunction(e.random));
                    break;
                }
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
            case Item.MotionSensor:
                sr.sprite = motionSensorSprite;
                break;
            case Item.SmokeGrenade:
                sr.sprite = smokeGrenadeSprite;
                break;
        }
    }

}
