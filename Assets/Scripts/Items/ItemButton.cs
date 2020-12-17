using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;
using System;

using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static ItemButton instance;


    [HideInInspector]public enum Item { None, Camera, MotionSensor, SmokeGrenade };
    [HideInInspector]public Item myItem;

    public Player player;
    public GameController gc;
    public TextMeshProUGUI text2;
    public GameObject buttonItemImage;
    public EvidenceHandler evidenceHandler;
    public GameObject ItemContainers;
    public TextMeshProUGUI infoText;
    private Image myItemImage;
    private Image backgroundImage;
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
    Calculation Enter;
    Calculation Exit;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        myItemImage = buttonItemImage.GetComponent<Image>();
        backgroundImage = GetComponent<Image>();
        //  SetItem(UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length)));
        SetItem(0);
        EventCallbacks.EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);

        if(ItemContainers == null)
        {
            ItemContainers = GameObject.Find("ItemContainers");
        }
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
        foreach (Transform child in ItemContainers.transform)
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
                message.random = UnityEngine.Random.Range(1, Enum.GetValues(typeof(Item)).Length);
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
           // GetComponent<AudioSource>().Play();
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
        gc.player.ResetArrows();
        gc.player.MotionSensorCheck = false;
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

        text2.text = "Item";
        myItemImage.enabled = true;
        backgroundImage.enabled = true;
        Exit = () => Empty();
        Enter = () => Empty();
        switch (myItem)
        {
            case Item.None:
                Debug.Log("None");
                Click = () => NoneClick();
                myItemImage.enabled = false;
                backgroundImage.enabled = false;
                text2.text = "";
                infoText.text = "";
                break;
            case Item.Camera:
                Click = () => CameraClick();
                myItemImage.sprite = CameraSprite;
                photosTaken = 0;
                infoText.text = "Takes a picture that can be presented during meetings";
                break;
            case Item.MotionSensor:
                Click = () => MotionSensorClick();
                myItemImage.sprite = buttonMotionSensorImage;
                Exit = () => MotionSensorExit();
                Enter = () => MotionSensorEnter();
                infoText.text = "Puts out a motion sensor. Each other player that walks through it will be compiled into a list that can be presented during meetings";
                break;
            case Item.SmokeGrenade:
                Click = () => SmokeGrenadeClick();
                myItemImage.sprite = SmokeGrenadeSprite;
                infoText.text = "Throws a smoke grenade that covers the area around you with smoke. Hide yourself or distract other players with it";
                break;
        }
    }
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Enter();
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Exit();
    }
    void MotionSensorEnter()
    {
        gc.player.MotionSensorCheck = true;
    }
    void MotionSensorExit()
    {

        gc.player.MotionSensorCheck = false;
        gc.player.ResetArrows();
    }
    void Empty()
    {
        Debug.Log("Empty");
    }
    //Start of game cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.previous == GamePhase.Setup)
        {
           int r = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));

            Debug.Log((Enum.GetValues(typeof(Item)).Length - 1) + "Enum");
            SetItem(0);
        }
    }
}
