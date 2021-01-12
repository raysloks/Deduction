using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using EventCallbacks;

public class ItemContainer : Interactable
{

    [HideInInspector] public enum Item { None, Camera, MotionSensor, SmokeGrenade, PulseChecker, Knife };
    [HideInInspector] public Item item;
    private bool waitingForCountDown = false;
    private bool coolingDown = false;
    public TextMeshPro text;
    public SpriteRenderer sr;
    public Sprite cameraSprite;
    public Sprite motionSensorSprite;
    public Sprite smokeGrenadeSprite;
    public Sprite knifeSprite;
    public Sprite pulseSprite;
    public GameController gc;

    void Start()
    {

        int r = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));
        if (!gc.settings.addKnifeItem && r == 5)
        {
            r = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 2));
        }
        Restock(r);
        EventSystem.Current.RegisterListener(EVENT_TYPE.PICKUP_WAIT, WaitForRestock);
        
        
        
        

        Debug.Log("Sibling Index : " + transform.GetSiblingIndex() + " Random+ " + (Enum.GetValues(typeof(Item)).Length - 2));
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

    public void WaitForRestock(EventCallbacks.Event eventInfo)
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
            case Item.PulseChecker:
                sr.sprite = pulseSprite;
                break;
            case Item.Knife:
                sr.sprite = knifeSprite;
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
                Debug.Log((Enum.GetValues(typeof(Item)).Length) + "Enum");

                int r = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));
                if (!gc.settings.addKnifeItem && r == 5)
                {
                    r = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 2));
                }

                PickupCooldown message = new PickupCooldown();
                message.child = transform.GetSiblingIndex();
                message.random = r;
                game.handler.link.Send(message);
            }
        
    }


    public override bool CanInteract(GameController game)
    {
        return !coolingDown && game.player.IsAlive;
    }

    public override void Interact(GameController game)
    {
        NoneClick(game);
    }
}