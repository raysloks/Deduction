﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using EventCallbacks;

public class ItemContainer : Interactable
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
    public SpriteRenderer outline;

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

    private void NoneClick(GameController game)
    {
            int item2 = (int)item;
            if (item2 != 0)
            {
                game.itemButton.GetComponent<ItemButton>().SetItem(item2);

                ItemTaken();
                PickupCooldown message = new PickupCooldown();
                message.child = transform.GetSiblingIndex();
                message.random = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));
                game.handler.link.Send(message);
            }
        
    }


    public override bool CanInteract(GameController game)
    {
        return !coolingDown;
     //   return alwaysActive || game.taskManager.tasks.Find(x => x.minigame_index == minigame_index && !x.completed) != null && game.player.role == 0 ||
     //       game.taskManager.sabotageTasks.Find(x => x.minigame_index == minigame_index) != null;
    }

    public override void Interact(GameController game)
    {
        NoneClick(game);
      //  game.popup.ActivatePopup(minigame, this);
    }

    public override void Highlight(bool highlighted)
    {
        
        if (outline != null)
        {
            if (highlighted)
                StartCoroutine(FadeInOutline(0.2f));
            else
                StartCoroutine(FadeOutOutline(0.2f));
        }
    }

    IEnumerator FadeInOutline(float seconds)
    {
        float counter = 0;

        while (counter < seconds)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOutOutline(float seconds)
    {
        float counter = seconds;

        while (counter > 0f)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter -= Time.deltaTime;
            yield return null;
        }
    }

}
