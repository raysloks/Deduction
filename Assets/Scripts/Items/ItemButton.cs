using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;
using System;

public class ItemButton : MonoBehaviour
{
    private static ItemButton instance;


    [HideInInspector]public enum Item { None, Camera, MotionSensor, SmokeGrenade };
    [HideInInspector]public Item myItem;

    public GameObject itemContainer;
    public GameObject player;
    public GameController gc;
    public TextMeshProUGUI text;
    public GameObject buttonItemImage;
    public EvidenceHandler evidenceHandler;
    private Image myItemImage;

    [Header("MotionSensor")]
    public GameObject motionSensorPrefab;
    public Sprite buttonMotionSensorImage;

    [Header("Camera")]
    public Sprite CameraSprite;
    private int maxPhotos = 3;
    private int photosTaken = 0;

    [Header("SmokeGrenade")]
    public Sprite SmokeGrenadeSprite;

    delegate void Calculation();
    Calculation Click;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        myItemImage = buttonItemImage.GetComponent<Image>();
      //  SetItem(UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1)));

        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }
    
    public void ItemClick()
    {
        Click();
    }
	
    //if you got no items
    private void NoneClick()
    {
        float closestDistance = 2f;
        GameObject childGo = null;
        int index = 0;
        foreach (Transform child in itemContainer.transform)
        {
            if (Vector2.Distance(child.position, player.transform.position) < closestDistance)
            {
                childGo = child.gameObject;
                closestDistance = Vector2.Distance(child.position, player.transform.position);
                Debug.Log("Close " + Vector2.Distance(child.position, player.transform.position));
                break;
            }
            index++;
        }
        if (childGo != null)
        {
            int item = (int)childGo.GetComponent<ItemContainer>().item;
            if (item != 0)
            {
                SetItem(item);

                childGo.GetComponent<ItemContainer>().ItemTaken();
                PickupCooldown message = new PickupCooldown();
                message.child = index;
                message.random = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));
                gc.handler.link.Send(message);
            }
        }
    }
	
    //if you got camera item
    private void CameraClick()
    {
        if (photosTaken < maxPhotos)
        {
            gc.handler.link.Send(new TakePhoto());
            photosTaken++;
        }
        if (photosTaken >= maxPhotos)
        {
            SetItem(0);
        }
    }

    private void MotionSensorClick()
    {
        GameObject ms = Instantiate(motionSensorPrefab, player.transform.position, Quaternion.identity);
        CheckSensor cs = ms.GetComponent<CheckSensor>();
        cs.gc = gc;
        cs.evidenceHandler = evidenceHandler;
        SetItem(0);
    }

    private void SmokeGrenadeClick()
    {
        SmokeGrenadeActivate message = new SmokeGrenadeActivate();
        message.pos = player.transform.position;
        gc.handler.link.Send(message);
        SetItem(0);
    }

    //set item based on Enum int
    public void SetItem(int item)
    {
        myItem = (Item)item;

        text.text = "";
        myItemImage.enabled = true;
        switch (myItem)
        {
            case Item.None:
                Click = () => NoneClick();
                myItemImage.enabled = false;
                text.text = "Pickup";
                break;
            case Item.Camera:
                Click = () => CameraClick();
                myItemImage.sprite = CameraSprite;
                photosTaken = 0;
                break;
            case Item.MotionSensor:
                Click = () => MotionSensorClick();
                myItemImage.sprite = buttonMotionSensorImage;
                break;
            case Item.SmokeGrenade:
                Click = () => SmokeGrenadeClick();
                myItemImage.sprite = SmokeGrenadeSprite;
                break;
        }
    }

    //Start of game cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.previous == GamePhase.Setup)
        {
           int r = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));
            if(r == 2)
            {
                r++;
            }
            SetItem(r);
         //   SetItem(2);
        }
    }
}
